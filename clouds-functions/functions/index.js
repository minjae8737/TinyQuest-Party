const { onCall, HttpsError } = require("firebase-functions/v2/https");
const admin = require("firebase-admin");

admin.initializeApp();

const db = admin.firestore();

const GACHA_TABLE = [
    { unitName: "Armored_Axeman", grade: "R",   weight: 74 },
    { unitName: "Soldier",        grade: "R",   weight: 74 },
    { unitName: "Knight",         grade: "R",   weight: 74 },
    
    { unitName: "Archer",         grade: "SR",  weight: 25 },
    { unitName: "Swordsman",      grade: "SR",  weight: 25 },
    { unitName: "Lancer",         grade: "SR",  weight: 25 },
    
    { unitName: "Knight_Templar", grade: "SSR", weight: 1  },
    { unitName: "Wizard",         grade: "SSR", weight: 1  },
    { unitName: "Priest",         grade: "SSR", weight: 1  },
];

const COSTS = {
    1: 1000,
    10: 10000,
    100: 100000,
    300: 300000
};

const PITY_THRESHOLD = 90;

// SSR 보장이 필요한 경우 테이블을 교체
const SSR_ONLY_TABLE = GACHA_TABLE.filter(u => u.grade === "SSR");

function weightedRandom(table) {
    const total = table.reduce((sum, item) => sum + item.weight, 0);
    let rand = Math.random() * total;
    
    for (const item of table) {
        rand -= item.weight;
        if (rand <= 0) return item;
    }
    
    return table[table.length - 1];
}

function drawOnce(currentPity) {
    if (currentPity >= PITY_THRESHOLD - 1) {
        const result = weightedRandom(SSR_ONLY_TABLE);
        return { result, resetPity: true };
    }
    
    const result = weightedRandom(GACHA_TABLE);
    const resetPity = result.grade === "SSR";
    
    return { result, resetPity };
}

exports.gacha = onCall(async (request) => {
    if (!request.auth) throw new HttpsError("unauthenticated", "로그인이 필요합니다");

    const uid = request.auth.uid;
    const count = request.data?.count ?? 1;

    if (!COSTS[count]) {
        throw new HttpsError("invalid-argument", "유효하지 않은 뽑기 수입니다");
    }

    const cost = COSTS[count];
    const docRef = db.collection("players").doc(uid);
    const snapshot = await docRef.get();

    if (!snapshot.exists) throw new HttpsError("not-found", "유저 없음");

    const playerData = snapshot.data();
    if (playerData.CurrencySaveData.Gold < cost) {
        throw new HttpsError("failed-precondition", "골드가 부족합니다");
    }

    // 현재 pity 카운터 가져오기
    let pityCount = playerData.GachaSaveData?.PityCount ?? 0;

    const results = [];

    for (let i = 0; i < count; i++) {
        const { result, resetPity } = drawOnce(pityCount);
        results.push(result);

        if (resetPity) {
            pityCount = 0;        // SSR 나오면 리셋
        } else {
            pityCount++;
        }
    }

    // Gold 차감 + pity 갱신을 한 번에
    await docRef.update({
        "CurrencySaveData.Gold":    admin.firestore.FieldValue.increment(-cost),
        "GachaSaveData.PityCount":  pityCount,
    });

    return {
        results:    results.map(r => ({ unitName: r.unitName, grade: r.grade })),
        totalCost:  cost,
        pityCount:  pityCount,
    };
});
const { onCall, HttpsError } = require("firebase-functions/v2/https");
const admin = require("firebase-admin");

admin.initializeApp();

const db = admin.firestore();

const GACHA_TABLE = [
    { unitName: "Armored_Axeman", grade: "R",   weight: 70 },
    { unitName: "Soldier",        grade: "R",   weight: 70 },
    { unitName: "Knight",         grade: "R",   weight: 70 },
    { unitName: "Archer",         grade: "SR",  weight: 25 },
    { unitName: "Swordsman",      grade: "SR",  weight: 25 },
    { unitName: "Lancer",         grade: "SR",  weight: 25 },
    { unitName: "Knight_Templar", grade: "SSR", weight: 5  },
    { unitName: "Wizard",         grade: "SSR", weight: 5  },
    { unitName: "Priest",         grade: "SSR", weight: 5  },
];

function weightedRandom(table) {
    const total = table.reduce((sum, item) => sum + item.weight, 0);
    let rand = Math.random() * total;
    for (const item of table) {
        rand -= item.weight;
        if (rand <= 0) return item;
    }
    return table[table.length - 1];
}

exports.gacha = onCall(async (request) => {
    if (!request.auth) {
        throw new HttpsError("unauthenticated", "로그인이 필요합니다");
    }

    const uid = request.auth.uid;
    const cost = 100;
    const docRef = db.collection("players").doc(uid);
    const snapshot = await docRef.get();

    if (!snapshot.exists) {
        throw new HttpsError("not-found", "유저 없음");
    }

    const playerData = snapshot.data();

    if (playerData.CurrencySaveData.Gold < cost) {
        throw new HttpsError("failed-precondition", "골드가 부족합니다");
    }

    const result = weightedRandom(GACHA_TABLE);

    await docRef.update({
        "CurrencySaveData.Gold": admin.firestore.FieldValue.increment(-cost)
    });

    return {
        unitName: result.unitName,
        grade: result.grade,
    };
});
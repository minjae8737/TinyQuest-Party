using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using UnityEngine;

public class FirebaseAuthManager : Singleton<FirebaseAuthManager>
{
    private FirebaseAuth auth;
    
    public FirebaseUser CurrentUser => auth.CurrentUser;
    
    #region TestData
    
    string testmail = "testmail@gmail.com";
    string password = "password";

    #endregion
    
    public async Task<bool> InitAndSignIn()
    {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

        if (dependencyStatus != DependencyStatus.Available)
        {
            Debug.LogError($"Firebase 초기화 실패: {dependencyStatus}");
            return false;
        }
            
        auth = FirebaseAuth.DefaultInstance;
        Debug.Log("Firebase 초기화 완료");
        
#if UNITY_EDITOR
        // 에디터에서는 고정 테스트 계정으로 로그인
        await Login(testmail, password);
        Debug.Log("에디터 테스트 계정 로그인");
#else
        await SignInAnonymously();
#endif
        return true;
    }
    
    public async Task SignInAnonymously()
    {
        var result = await auth.SignInAnonymouslyAsync(); // auth.CurrentUser에 자동으로 세팅
        Debug.Log($"게스트 UID: {result.User.UserId}");
    }
    
    public async Task Register(string email, string password)
    {
        var result = await auth
            .CreateUserWithEmailAndPasswordAsync(email, password);
        Debug.Log($"회원가입 성공: {result.User.Email}");
    }

    public async Task Login(string email, string password)
    {
        var result = await auth
            .SignInWithEmailAndPasswordAsync(email, password);
        Debug.Log($"로그인 성공: {result.User.Email}");
    }

    public void Logout()
    {
        auth.SignOut();
        Debug.Log("로그아웃");
    }
}

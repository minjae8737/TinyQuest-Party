using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using UnityEngine;

public class FirebaseAuthManager : Singleton<FirebaseAuthManager>
{
    private FirebaseAuth auth;
    
    public FirebaseUser CurrentUser => auth.CurrentUser;
    
    public bool IsEmailLinked
    {
        get
        {
            if (CurrentUser == null) return false;
            foreach (var info in CurrentUser.ProviderData)
                if (info.ProviderId == "password") return true; // "password" 이메일 로그인 (ex. 익명 = "firebase") 
            return false;
        }
    }
    
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
        await LoginWithEamil(testmail, password);
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

    public async Task LinkWithEmail(string email, string password)
    {
        if (CurrentUser == null) throw new Exception("로그인 상태가 아닙니다");

        var credential = EmailAuthProvider.GetCredential(email, password);
    
        try
        {
            var result = await CurrentUser.LinkWithCredentialAsync(credential);
            Debug.Log($"이메일 연동 성공: {result.User.Email}");
        }
        catch (FirebaseException e)
        {
            throw new Exception(GetErrorMessage(e));
        }
    }

    public async Task LoginWithEamil(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(password))
        {
            throw new Exception("이메일과 비밀번호를 입력해주세요");   
        }
        
        try
        {
            var result = await auth
                .SignInWithEmailAndPasswordAsync(email, password);
            Debug.Log($"로그인 성공: {result.User.Email}");
        }
        catch (FirebaseException e)
        {
            throw new Exception(GetErrorMessage(e));;
        }
    }

    public void Logout()
    {
        auth.SignOut();
        Debug.Log("로그아웃");
    }
    
    private string GetErrorMessage(FirebaseException e)
    {
        switch ((AuthError)e.ErrorCode)
        {
            case AuthError.InvalidEmail: return "이메일 형식이 올바르지 않습니다";
            case AuthError.EmailAlreadyInUse: return "이미 사용 중인 이메일입니다";
            case AuthError.WeakPassword: return "비밀번호는 6자 이상이어야 합니다";
            case AuthError.UserNotFound: return "존재하지 않는 계정입니다";
            case AuthError.WrongPassword: return "비밀번호가 틀렸습니다";
            default: return "오류가 발생했습니다";
        }
    }
}

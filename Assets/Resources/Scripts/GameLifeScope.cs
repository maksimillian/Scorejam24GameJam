using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameLifeScope : MonoBehaviour
{
    private Authentication _authentication;
    private ILeaderboard _leaderboard;
    private void Start()
    {
        _authentication = gameObject.GetComponent<Authentication>();
        _leaderboard = gameObject.GetComponent<ILeaderboard>();
        PlayMusic().Forget();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
    
    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    async UniTask PlayMusic()
    {
        UniTask.DelayFrame(3);
        Debug.Log("Music Started Playing");
        Sounds.instance.PlaySound(Sounds.Audio.StartGame);
        UniTask.DelayFrame(3);
        await UniTask.WaitUntil(() => Sounds.instance.audioSource.isPlaying == false);
        Sounds.instance.PlayLoop(Sounds.Audio.LoopGame);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Pause();
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            //Pause();
        }
    }

    private void Pause()
    {
        Time.timeScale = 0f;
    }

    private void Unpause()
    {
        Time.timeScale = 1f;
    }

    public void SetScore(int score)
    {
        StartCoroutine(_leaderboard.SubmitScoreRoutine(score));
    }
}
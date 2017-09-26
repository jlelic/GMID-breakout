using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GamePlayHandler : MonoBehaviour
{
    public Text BoxesLeftText;
    public Text BallsLeftText;
    public GameObject[] Levels;
    public GameObject ExitButton;
    public GameObject Ball;
    public GameObject TextMessagePrefab;
    public GameObject PausePanel;
    public GameObject IntroText;
    public AudioClip GameWonAudioClip;
    public AudioClip GameLostAudioClip;
    public AudioClip BallLostAudioClip;
    public Color TextMessageColor;

    private int _boxesLeft;
    private int _ballsLeft;
    private bool _paused = true;
    private GameObject[] _buttons;
    private GameObject _player;
    private bool _gameCompleted = false;
    private AudioSource _audioSource;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _audioSource = GetComponentInChildren<AudioSource>();
        ExitButton.GetComponent<Button>().onClick.AddListener(ExitGame);
        var buttonPos = ExitButton.transform.position;
        buttonPos.y += 300;

        _buttons = new GameObject[Levels.Length + 1];
        _buttons[0] = ExitButton;
        for (int i = 0; i < Levels.Length; i++)
        {
            var button = GameObject.Instantiate(ExitButton);
            _buttons[i + 1] = button;
            button.transform.parent = ExitButton.transform.parent;
            buttonPos.y -= 50;
            button.transform.position = buttonPos;
            button.GetComponentInChildren<Text>().text = "Level " + (i + 1);
            button.GetComponent<Button>().onClick.AddListener(GetLoadLevelDelegate(i));
        }
        PauseGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpauseGame(!_paused);
        }
        if (Input.GetKeyDown(KeyCode.Space) && _paused)
        {
            UnpauseGame();
        }
    }

    public void ExitGame()
    {
        Debug.Log("EXIT");
        Application.Quit();
    }

    UnityAction GetLoadLevelDelegate(int x)
    {
        return delegate() { LoadLevel(x); };
    }

    void LoadLevel(int x)
    {
        Destroy(GameObject.FindGameObjectWithTag("Level"));
        foreach (var ball in GameObject.FindGameObjectsWithTag("Ball"))
        {
            Destroy(ball);
        }
        foreach (var box in GameObject.FindGameObjectsWithTag("Box"))
        {
            Destroy(box);
        }
        _ballsLeft = 0;
        _boxesLeft = 0;
        _gameCompleted = false;
        Instantiate(Levels[x], Vector3.zero, Quaternion.identity);
        Instantiate(Ball, new Vector3(0, 0, -6.35f), Quaternion.identity);
        SetupCounters();
    }

    void PauseGame()
    {
        PauseUnpauseGame(true);
    }

    void UnpauseGame()
    {
        PauseUnpauseGame(false);
    }

    void PauseUnpauseGame(bool pause)
    {
        _paused = pause;
        if (_paused)
        {
            Time.timeScale = 0f;
            _paused = true;
        }
        else
        {
            Time.timeScale = 1f;
            _paused = false;
        }
        foreach (var button in _buttons)
        {
            button.SetActive(_paused);
        }
        foreach (var ball in GameObject.FindGameObjectsWithTag("Ball"))
        {
            ball.GetComponents<AudioSource>()[0].mute = _paused;
        }
        PausePanel.SetActive(_paused);
        IntroText.SetActive(_paused);

    }

    void SetupCounters()
    {
        BallsLeftText.text = "Balls: " + _ballsLeft.ToString();
        BoxesLeftText.text = "Boxes: " + _boxesLeft.ToString();
    }

    public void OnBoxAdded()
    {
        _boxesLeft++;
        BoxesLeftText.text = "Boxes: " + _boxesLeft.ToString();
    }

    public void OnBoxDestroyed()
    {
        _boxesLeft--;
        BoxesLeftText.text = "Boxes: " + _boxesLeft.ToString();
        if (_boxesLeft <= 0 && !_gameCompleted)
        {
            StartCoroutine(LevelCompleted());
        }

    }

    public void OnBallAdded()
    {
        _ballsLeft++;
        BallsLeftText.text = "Balls: " + _ballsLeft.ToString();
    }

    public void OnBallDestroyed()
    {
        _ballsLeft--;
        BallsLeftText.text = "Balls: " + _ballsLeft.ToString();
        if (_ballsLeft <= 0 && !_gameCompleted)
        {
            StartCoroutine(GameOver());
        }
        _audioSource.clip = BallLostAudioClip;
        _audioSource.Play();
    }

    IEnumerator GameOver()
    {
        _gameCompleted = true;
        var textMesh = GameObject.Instantiate(TextMessagePrefab, Vector3.zero, Quaternion.identity).GetComponent<TextMesh>();
        textMesh.text = "GAME OVER";
        textMesh.color = TextMessageColor;
        yield return new WaitForSeconds(1.2f);
        _audioSource.clip = GameLostAudioClip;
        _audioSource.Play();
        yield return new WaitForSeconds(2.8f);
        PauseGame();
    }

    IEnumerator LevelCompleted()
    {
        _gameCompleted = true;
        var textMesh = GameObject.Instantiate(TextMessagePrefab, Vector3.zero, Quaternion.identity).GetComponent<TextMesh>();
        textMesh.text = "LEVEL COMPLETED";
        textMesh.color = TextMessageColor;
        _audioSource.clip = GameWonAudioClip;
        _audioSource.Play();
        yield return new WaitForSeconds(4);
        PauseGame();
    }
}
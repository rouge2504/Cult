using UnityEngine;
using UnityEngine.Events;

public enum TweenType { Scale, Move, Rotate, MoveZ, MoveY, MoveX, MoveUp, None }

public class GenericButtonTween : MonoBehaviour
{
    public float delay;
    public float duration;
    public LeanTweenType easeType;
    public TweenType tweenType;
    public float from;
    public float to;
    public int loopCount;

    public UnityEvent PreTweenEvent;
    public UnityEvent PostTweenEvent;

    private void Awake()
    {
        if (PreTweenEvent != null)
        {
            PreTweenEvent.Invoke();
        }

        if (from > 0F)
        {
            transform.localScale = new Vector3(from, from, from);
        }
    }

    private void Start()
    {
        if (tweenType == TweenType.Scale)
        {
            LeanTween.scale(this.gameObject, new Vector3(to, to, to), duration).setDelay(delay).setEase(easeType).setLoopCount(loopCount).setOnComplete(() =>
            {
                if (PostTweenEvent != null)
                {
                    PostTweenEvent.Invoke();
                }
            });
        }
        else if (tweenType == TweenType.Move)
        {
            LeanTween.move(this.gameObject, new Vector3(to, to, to), duration).setDelay(delay).setEase(easeType).setLoopCount(loopCount).setOnComplete(() =>
            {
                if (PostTweenEvent != null)
                {
                    PostTweenEvent.Invoke();
                }
            });
        }

        else if (tweenType == TweenType.MoveZ)
        {
            LeanTween.move(this.gameObject, new Vector3(0, 0, to), duration).setDelay(delay).setEase(easeType).setLoopCount(loopCount).setOnComplete(() =>
            {
                if (PostTweenEvent != null)
                {
                    PostTweenEvent.Invoke();
                }
            });
        }

        else if (tweenType == TweenType.MoveUp)
        {
            LeanTween.move(this.gameObject, new Vector3(this.gameObject.transform.position.x, to, this.gameObject.transform.position.z - 2), duration).setDelay(delay).setEase(easeType).setLoopCount(loopCount).setOnComplete(() =>
            {
                if (PostTweenEvent != null)
                {
                    PostTweenEvent.Invoke();
                }
            });
        }

        else if (tweenType == TweenType.MoveX)
        {
            LeanTween.move(this.gameObject, new Vector3(to, this.gameObject.transform.position.y, this.gameObject.transform.position.z), duration).setDelay(delay).setEase(easeType).setLoopCount(loopCount).setOnComplete(() =>
            {
                if (PostTweenEvent != null)
                {
                    PostTweenEvent.Invoke();
                }
            });
        }

        else if (tweenType == TweenType.Rotate)
        {
            LeanTween.rotate(this.gameObject, new Vector3(to, to, to), duration).setDelay(delay).setEase(easeType).setLoopCount(loopCount).setOnComplete(() =>
            {
                if (PostTweenEvent != null)
                {
                    PostTweenEvent.Invoke();
                }
            });
        }
    }

    public void MoveField()
    {
        if (tweenType == TweenType.MoveZ)
        {
            LeanTween.move(this.gameObject, new Vector3(0, 0, to), duration).setDelay(delay).setEase(easeType).setLoopCount(loopCount).setOnComplete(() =>
            {
                if (PostTweenEvent != null)
                {
                    PostTweenEvent.Invoke();
                }
            });
        }
    }

    public void MoveCube(Vector3 pos)
    {
        if (tweenType == TweenType.MoveY)
        {
            LeanTween.move(this.gameObject, pos, duration).setDelay(delay).setEase(easeType).setLoopCount(loopCount).setOnComplete(() =>
            {
                if (PostTweenEvent != null)
                {
                    PostTweenEvent.Invoke();
                }
            });
        }
    }


    public void DestroyMe()
    {
        Destroy(this.gameObject);
    }

    public void DestroyMe(int delay)
    {
        Destroy(this.gameObject, delay);
    }
}
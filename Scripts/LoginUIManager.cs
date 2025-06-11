using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Mingle.Dev.Scripts.UI.Login_Renewal.View;
using UnityEngine;
using UnityEngine.UI;

namespace Mingle.Dev.Scripts.Login_Renewal.Manager
{
    public class LoginUIManager : MonoBehaviour
    {
        [SerializeField] private Canvas pageCanvas;
        [SerializeField] private Canvas popupCanvas;
        [SerializeField] private Image dimBackground;
        
        private PopupView _activePopup;
        
        private Dictionary<Type, UIView> UIViews { get; set; }
        private Stack<UIView> _history;
        private UIView CurrentView => _history.Count > 0 ? _history.Peek() : null;

        private static LoginUIManager _instance;
        public static LoginUIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<LoginUIManager>();
                    if (_instance == null)
                    {
                        Debug.Log("UIManager instance not found!");
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            InitializeSingleton();
            InitializeManager();
        }
        
        private void InitializeSingleton()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        private void InitializeManager()
        {
            UIViews = pageCanvas.GetComponentsInChildren<UIView>(true)
                .Concat(popupCanvas.GetComponentsInChildren<UIView>(true))
                .ToDictionary(view => view.GetType(), view => view);
            
            _history = new Stack<UIView>();
        }
        
        public async UniTask Next<TView>() where TView : UIView
        {
            UIView targetView = UIViews[typeof(TView)];
            
            if (targetView is PopupView popupView)
            {
                _activePopup = popupView; // 직접 참조하고 FindObjectsOfType 삭제
                if (popupView.UseDimBackground)
                {
                    dimBackground.gameObject.SetActive(true);
                }
            }
            
            targetView.gameObject.SetActive(true); 
            await targetView.ShowAnimation();
            
            if (targetView is PageView)
            {
                // 활성화된 Popup이 있다면 비활성화
                if (_activePopup != null)
                {
                    await _activePopup.HideAnimation();
                    _activePopup.gameObject.SetActive(false);
                    dimBackground.gameObject.SetActive(false);
                }

                if (CurrentView != null)
                {
                    await CurrentView.HideAnimation();
                    CurrentView.gameObject.SetActive(false);
                }
                _history.Push(targetView);
            }
        }

        public async UniTask Previous()
        {
            Debug.Log(_history.Count);
            if (_history.Count == 0) return;
            
            await CurrentView.HideAnimation();
            CurrentView.gameObject.SetActive(false);
            
            if (CurrentView is PageView)
            {
                await CurrentView.ShowAnimation();
                _history.Pop();
                CurrentView.gameObject.SetActive(true);
            }
            
            if (_activePopup == null || !_activePopup.UseDimBackground)
            {
                dimBackground.gameObject.SetActive(false);
            }
        }

        // 팝업 닫기
        public void ClosePopup()
        {
            _activePopup.gameObject.SetActive(false);
            _activePopup = null; 
            dimBackground.gameObject.SetActive(false);
        }
        
        // 페이지 타입 가져오기(모든 곳에서 GetComponent를 사용하지 않아도 됨)
        public T GetPage<T>() where T : UIView
        {
            if (UIViews.TryGetValue(typeof(T), out var view))
            {
                return (T)view;
            }
            return null;
        }
        
        // dimBackground Z 위치 설정 메서드
        public void SetDimBackgroundZPos(float zPos)
        {
            dimBackground.transform.localPosition = new Vector3(0, 0, zPos);
        }
    }
}

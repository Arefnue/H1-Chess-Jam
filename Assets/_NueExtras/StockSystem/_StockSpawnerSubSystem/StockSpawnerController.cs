using System;
using System.Collections;
using System.Collections.Generic;
using _NueCore.Common.KeyValueDict;
using _NueCore.Common.ReactiveUtils;
using _NueCore.Common.Utility;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _NueExtras.StockSystem._StockSpawnerSubSystem
{
    public class StockSpawnerController : MonoBehaviour
    {

        [SerializeField,TabGroup("References")] private Canvas movingCanvas;
        [SerializeField,TabGroup("References")] private KeyValueDict<StockTypes,StockSpawnObject> stockSpawnObjectUIDict;
        [SerializeField,TabGroup("References")] private KeyValueDict<StockTypes,StockSpawnObject> stockSpawnObject3DDict;
       
        #region Cache
        public static Dictionary<StockTypes, Transform> StockMoveTargetDict { get; private set; } = new Dictionary<StockTypes, Transform>();
        private Camera Camera => CameraStatic.MainCamera;
        private Transform CameraTransform => Camera.transform;
        private int _movingObjectCount;
        private const int MaxSpawnCount = 30;
        #endregion

        #region Setup
        private void Awake()
        {
            RegisterREvents();
        }
        #endregion

        #region Reactive
        private void RegisterREvents()
        {
            RBuss.OnEvent<Move3DToUIWithPhysicGlobalREvent>().TakeUntilDisable(gameObject).Subscribe(
                ev =>
                {
                    StartCoroutine(Move3DToUIRoutineWitchPhysicCustom(ev.CustomStock,ev.TargetRoot,(() =>
                    {
                        ev.FinishedAction?.Invoke();
                    }),ev.DownScaleRate));
                });
            
            RBuss.OnEvent<StockSpawnerREvents.SetDefaultSpawnTargetREvent>().Subscribe(ev =>
            {
                SetDefaultStockMoveTarget(ev.StockType, ev.TargetRoot);
            });

            RBuss.OnEvent<StockSpawnerREvents.SpawnStockREvent>().Subscribe(ev =>
            {
                Transform targetRoot = null;
                
                if (StockMoveTargetDict.TryGetValue(ev.StockType, out var value))
                    targetRoot = value;
                
                var clone = SpawnStock(ev.StockType,targetRoot, ev.FinishedAction,is3D: ev.Spawn3D);
                if (ev.HasStartPos)
                {
                    if (clone)
                    {
                        clone.transform.position = ev.Spawn3D ? ev.StartPos : Camera.WorldToScreenPoint(ev.StartPos);
                    }
                }
            });
            
            RBuss.OnEvent<StockSpawnerREvents.DeSpawnStockREvent>().Subscribe(ev =>
            {
                Transform spawnParent = null;
                
                if (StockMoveTargetDict.TryGetValue(ev.StockType, out var value))
                    spawnParent = value;

                if (spawnParent == null)
                {
                    spawnParent = transform;
                }
                
                var clone = DeSpawnStock(ev.StockType,ev.TargetTransform, ev.FinishedAction,is3D: ev.Spawn3D,ev.IsLastParticle);
                clone.transform.position = spawnParent.position;
            });
        }
        
        public static void Move3DToUIWithPhysicGlobal(ICustomStock stockSpawnObject,Transform targetRoot,Action onFinishedAction = null,float downScaleRate = 0.5f)
        {
            RBuss.Publish(new Move3DToUIWithPhysicGlobalREvent(stockSpawnObject,targetRoot,onFinishedAction,downScaleRate));
        }
        
        public class Move3DToUIWithPhysicGlobalREvent : REvent 
        {
            public ICustomStock CustomStock { get; }
            public Transform TargetRoot { get; }
            public Action FinishedAction { get; }
            public float DownScaleRate { get; }
            public Move3DToUIWithPhysicGlobalREvent(ICustomStock customStock,Transform targetRoot,Action onFinishedAction = null,float downScaleRate = 0.5f)
            {
                CustomStock = customStock;
                TargetRoot = targetRoot;
                FinishedAction = onFinishedAction;
                DownScaleRate = downScaleRate;
            }
        }
        #endregion

        #region Spawns
        private StockSpawnObject SpawnStock(StockTypes targetType,Transform spawnParent,Action<StockSpawnObject> onFinishedAction = null,bool is3D = false)
        {
            var targetPrefab = is3D ? stockSpawnObject3DDict[targetType] : stockSpawnObjectUIDict[targetType];
            var clone =Spawn(targetPrefab, spawnParent, onFinishedAction,is3D: is3D);
            return clone;
        }
        private StockSpawnObject Spawn(StockSpawnObject movingPrefab,Transform spawnParent,Action<StockSpawnObject> onFinishedAction = null,float downScaleRate = 0.5f,float sphereRadius = 1f,bool is3D = false)
        {
            if (is3D)
            {
                var clone3D =Instantiate(movingPrefab, null);
                clone3D.Build(onFinishedAction);
                if (clone3D.UsePhysic)
                {
                    Move3DToUIWithPhysic(clone3D,spawnParent,() =>
                    {
                        PlaySfx();
                        clone3D.DestroyYourself();
                    },downScaleRate);
                }
                else
                {
                    Move3DToUI(clone3D.transform,spawnParent,(() =>
                    {
                        PlaySfx();
                        clone3D.DestroyYourself();
                    }),downScaleRate);
                }
              
                return clone3D;
            }
            
            var cloneUI =Instantiate(movingPrefab, movingCanvas.transform);
            
            cloneUI.Build(onFinishedAction);
            MoveUIToUI(cloneUI.transform,spawnParent, () =>
            {
                PlaySfx();
                cloneUI.DestroyYourself();
            },downScaleRate,sphereRadius);
            return cloneUI;
        }
        #endregion

        #region DeSpawns
        private StockSpawnObject DeSpawnStock(StockTypes targetType,Transform spawnParent,Action<StockSpawnObject> onFinishedAction = null,bool is3D = false,bool isLastParticle = false)
        {
            var targetPrefab = is3D ? stockSpawnObject3DDict[targetType] : stockSpawnObjectUIDict[targetType];
            var clone =DeSpawn(targetPrefab, spawnParent, onFinishedAction,is3D: is3D,isLastParticle: isLastParticle);
            return clone;
        }
        private StockSpawnObject DeSpawn(StockSpawnObject movingPrefab,Transform spawnParent,Action<StockSpawnObject> onFinishedAction = null,float downScaleRate = 0.5f,float sphereRadius = 1f,bool is3D = false,bool isLastParticle = false)
        {
            if (is3D)
            {
                var clone3D =Instantiate(movingPrefab, null);
                Move3DToUI(clone3D.transform,spawnParent,(() =>
                {
                    onFinishedAction?.Invoke(clone3D);
                    PlaySfx();
                    clone3D.DestroyYourself();
                }),downScaleRate);
                return clone3D;
            }
            
            var cloneUI =Instantiate(movingPrefab, movingCanvas.transform);
            MoveUIToUIDeSpawn(cloneUI.transform,spawnParent, () =>
            {
                onFinishedAction?.Invoke(cloneUI);
                PlaySfx();
                cloneUI.DestroyYourself();
            },downScaleRate,isLastParticle);
            return cloneUI;
        }

        #endregion

        #region Common Methods

        private bool _isSfxPlaying;
        private Tween _delayTween;
        private void PlaySfx()
        {
            if (_isSfxPlaying) return;
            _isSfxPlaying = true;
            _delayTween?.Kill();
            _delayTween = DOVirtual.DelayedCall(0.5f, () =>
            {
                _isSfxPlaying = false;
            },false);
        }

        
        private void SetDefaultStockMoveTarget(StockTypes targetType, Transform targetRoot)
        {
            if (StockMoveTargetDict.ContainsKey(targetType))
            {
                StockMoveTargetDict[targetType] = targetRoot;
            }
            else
            {
                StockMoveTargetDict.Add(targetType,targetRoot);
            }
        }
        #endregion
        
        #region UIToUIDespawn
        private void MoveUIToUIDeSpawn(Transform movingRoot,Transform targetRoot,Action onFinishedAction = null,float downScaleRate = 0.5f,bool isLastParticle = false)
        {
            if (_movingObjectCount>=MaxSpawnCount && !isLastParticle)
            {
                onFinishedAction?.Invoke();
            }
            else
            {
                StartCoroutine(MoveUIToUIRoutineDeSpawn(movingRoot,targetRoot,(() =>
                {
                    onFinishedAction?.Invoke();

                }),downScaleRate,isLastParticle));
            }
        }
        private IEnumerator MoveUIToUIRoutineDeSpawn(Transform movingRoot,Transform targetRoot,Action onFinishedAction = null,float downScaleRate = 0.5f,bool isLastParticle = false)
        {
            if (movingRoot)
                movingRoot.SetParent(targetRoot);
           
         
            _movingObjectCount++;
            
            var velocity = Vector3.zero;
            
            velocity.x += Random.Range(-1.0f, 1.0f);
            velocity.y += Random.Range(-2.0f, 2.0f);
            velocity *= 2f;
            
            var duration = 0.3f;
            var delay = Random.Range(0.2f,0.5f);
            if (isLastParticle)
            {
                delay = 0.65f;
            }
            
            bool isChecked = false;
            var delayTimer = 0f;
            var initScale = Vector3.zero;
            if (movingRoot)
            {
                initScale =movingRoot.localScale;
            }

            while (!isChecked)
            {
                delayTimer += Time.deltaTime;
                
                if (delayTimer > delay)
                {
                    delayTimer = delay;
                    isChecked = true;
                }
                
                velocity += new Vector3(0, -Time.deltaTime * 2.5f, 0);
                if (movingRoot == null)
                    break;
                movingRoot.localPosition += new Vector3(velocity.x, velocity.y, 0);
                
                var inverse = Mathf.InverseLerp(0, delay, delayTimer);
                movingRoot.localScale = Vector3.Lerp(initScale, Vector3.one, inverse);
                
                if (_movingObjectCount>=MaxSpawnCount && !isLastParticle)
                    break;
                yield return null;
            }
            
            var timer = 0f;
            isChecked = false;
            var initPos = Vector3.zero;
            if (movingRoot)
            {
                initPos = movingRoot.localPosition;
                initScale = movingRoot.localScale;
            }
           
            
            while (!isChecked)
            {
                timer += Time.deltaTime;
                
                if (timer > duration)
                {
                    timer = duration;
                    isChecked = true;
                }
            
                var inverse = Mathf.InverseLerp(0, duration, timer);
                if (movingRoot == null)
                    break;
                movingRoot.localPosition = Vector3.Lerp(initPos,Vector3.zero,inverse);
                movingRoot.localScale = Vector3.Lerp(initScale, downScaleRate * Vector3.one, inverse);
             
                if (_movingObjectCount>=MaxSpawnCount && !isLastParticle)
                    break;
                yield return null;
            }
            
            _movingObjectCount--;
            
            if (_movingObjectCount<=0)
                _movingObjectCount = 0;
           
            onFinishedAction?.Invoke();
        }
        #endregion
        
        #region UIToUI
        private void MoveUIToUI(Transform movingRoot,Transform targetRoot,Action onFinishedAction = null,float downScaleRate = 0.5f,float sphereRadius = 1f)
        {
            if (_movingObjectCount>=MaxSpawnCount)
            {
                onFinishedAction?.Invoke();
            }
            else
            {
                StartCoroutine(MoveUIToUIRoutine(movingRoot,targetRoot,(() =>
                {
                    onFinishedAction?.Invoke();

                }),downScaleRate,sphereRadius));
            }
        }
        private Vector3 GetUISpawnPos(float unitSphereRate = 1f)
        {
            Vector3 screenPos = Vector3.zero;
            
            var randomUnit = Random.insideUnitCircle* unitSphereRate;

            screenPos.x += Screen.width * randomUnit.x ;
            screenPos.y += Screen.height * randomUnit.y;

            return screenPos;
        }
        private IEnumerator MoveUIToUIRoutine(Transform movingRoot,Transform targetRoot,Action onFinishedAction = null,float downScaleRate = 0.5f,float unitSphereRate = 1f)
        {
            if (movingRoot)
                movingRoot.SetParent(targetRoot);
           
         
            _movingObjectCount++;
            
            var velocity = Vector3.zero;
            
            velocity.x += Random.Range(-2.0f, 2.0f);
            velocity.y += Random.Range(-2.0f, 2.0f);
            velocity *= 2f;
            
            var duration = 0.3f;
            var delay = Random.Range(0.5f,1f);
            
            bool isChecked = false;
            var delayTimer = 0f;
            var initScale = movingRoot.localScale;

            while (!isChecked)
            {
                delayTimer += Time.deltaTime;
                
                if (delayTimer > delay)
                {
                    delayTimer = delay;
                    isChecked = true;
                }
                
                velocity += new Vector3(0, Time.deltaTime * 5.5f, 0);
                if (movingRoot == null)
                    break;
                movingRoot.localPosition += new Vector3(velocity.x, velocity.y, 0);
                
                var inverse = Mathf.InverseLerp(0, delay, delayTimer);
                movingRoot.localScale = Vector3.Lerp(initScale, Vector3.one*1.5f, inverse);
                
                if (_movingObjectCount>=MaxSpawnCount)
                    break;
                yield return null;
            }
            
            var timer = 0f;
            isChecked = false;
            
            
            var initPos = movingRoot ?movingRoot.localPosition : Vector3.zero;
            initScale = movingRoot? movingRoot.localScale : Vector3.zero;

            
            while (!isChecked)
            {
                timer += Time.deltaTime;
                
                if (timer > duration)
                {
                    timer = duration;
                    isChecked = true;
                }
            
                var inverse = Mathf.InverseLerp(0, duration, timer);
                if (movingRoot == null)
                    break;
                movingRoot.localPosition = Vector3.Lerp(initPos,Vector3.zero,inverse);
                movingRoot.localScale = Vector3.Lerp(initScale, downScaleRate * Vector3.one, inverse);
             
                if (_movingObjectCount>=MaxSpawnCount)
                    break;
                yield return null;
            }
            
            _movingObjectCount--;
            
            if (_movingObjectCount<=0)
                _movingObjectCount = 0;
           
            onFinishedAction?.Invoke();
        }
        #endregion
        
        #region 3DToUI
        private void Move3DToUI(Transform movingRoot,Transform targetRoot,Action onFinishedAction = null,float downScaleRate = 0.5f)
        {
            if (_movingObjectCount>=MaxSpawnCount)
            {
                onFinishedAction?.Invoke();
            }
            else
            {
                StartCoroutine(Move3DToUIRoutine(movingRoot,targetRoot,(() =>
                {
                    onFinishedAction?.Invoke();

                }),downScaleRate));
            }
        }
        private void Move3DToUIWithPhysic(StockSpawnObject stockSpawnObject,Transform targetRoot,Action onFinishedAction = null,float downScaleRate = 0.5f)
        {
            if (_movingObjectCount>=MaxSpawnCount)
            {
                onFinishedAction?.Invoke();
            }
            else
            {
                StartCoroutine(Move3DToUIRoutineWitchPhysic(stockSpawnObject,targetRoot,(() =>
                {
                    onFinishedAction?.Invoke();

                }),downScaleRate));
            }
        }
        private (Vector3, Transform) Get3DMovePos(Transform target)
        {
            if (Camera != null)
            {
                var ray1 = Camera.ScreenPointToRay(target.position);
                var point1 = ray1.origin + ray1.direction * 5f;
                
                if (CameraTransform)
                    point1 = CameraTransform.InverseTransformPoint(point1);
                
                return (point1, CameraTransform);
            }

            return (Vector3.zero, null);
        }
        private IEnumerator Move3DToUIRoutine(Transform movingRoot,Transform targetRoot,Action onFinishedAction = null,float downScaleRate = 0.5f)
        {
            yield return null;
            
            
            var collectTuple = Get3DMovePos(targetRoot);
            movingRoot.SetParent(collectTuple.Item2);
            
            
            var initWorldPos = movingRoot.position;
            var initScale = movingRoot.localScale;
            _movingObjectCount++;
            
            var velocity = Vector3.zero;
            
            velocity.x += Random.Range(-1.0f, 1.0f);
            velocity.y += Random.Range(0.2f, 0.7f);
            velocity.z += Random.Range(-1.0f, 1.0f);

            var delayMin = 1f;
            var delayMax = 2f;
            var delayDuration = Random.Range(delayMin,delayMax);
            bool isChecked = false;
            var delayTimer = 0f;

            while (!isChecked)
            {
                delayTimer += Time.deltaTime;
                
                if (delayTimer > delayDuration)
                {
                    delayTimer = delayDuration;
                    isChecked = true;
                }
                
                velocity += new Vector3(Time.deltaTime * 1f, -Time.deltaTime * 1f, Time.deltaTime * 1f);
                movingRoot.position = NMath.SmoothCurveY(initWorldPos, initWorldPos + velocity, 1f, EaseHelper.EaseInSine(delayTimer, delayDuration));;
                
                var inverse = Mathf.InverseLerp(0, delayDuration, delayTimer);
                movingRoot.localScale = Vector3.Lerp(initScale, Vector3.one*1.5f, inverse);
                
                if (_movingObjectCount>=MaxSpawnCount)
                    break;
                
                yield return null;
            }
            
            var moveTimer = 0f;
            isChecked = false;
            var targetPos = collectTuple.Item1;
            var initPos = movingRoot.localPosition;
            initScale = movingRoot.localScale;
            var duration =Random.Range(0.5f,1f);

            while (!isChecked)
            {
                moveTimer += Time.deltaTime;
                if (moveTimer > duration)
                {
                    moveTimer = duration;
                    isChecked = true;
                }

                var t = EaseHelper.EaseInSine(moveTimer, duration);
                movingRoot.localPosition = NMath.SmoothCurveY(initPos, targetPos, 1f, t);
                movingRoot.localScale = Vector3.Lerp(initScale, downScaleRate * Vector3.one, t);

                if (_movingObjectCount>=MaxSpawnCount)
                    break;
                
                yield return null;
            }
            _movingObjectCount--;
            if (_movingObjectCount<=0)
                _movingObjectCount = 0;
           
            onFinishedAction?.Invoke();
        }
        private IEnumerator Move3DToUIRoutineWitchPhysic(StockSpawnObject stockSpawnObject,Transform targetRoot,Action onFinishedAction = null,float downScaleRate = 0.9f)
        {
            yield return null;
            
            
            var collectTuple = Get3DMovePos(targetRoot);
           
            _movingObjectCount++;
            
            var velocity = Vector3.zero;
            
            velocity.x += Random.Range(-0.3f, 0.3f);
            velocity.y += Random.Range(0, 0.2f);
            velocity.z += Random.Range(-0.3f, 0.3f);

            var delayMin = 0.2f;
            var delayMax = 0.3f;
            var delayDuration = Random.Range(delayMin,delayMax);
            bool isChecked = false;
            var delayTimer = 0f;
            // if (stockSpawnObject)
            // {
            //     stockSpawnObject.Rb.AddExplosionForce(8,initWorldPos + velocity,1,0.5f,ForceMode.VelocityChange);
            // }
            
            while (!isChecked)
            {
                delayTimer += Time.deltaTime;
                
                if (delayTimer > delayDuration)
                {
                    delayTimer = delayDuration;
                    isChecked = true;
                }
                
                //var inverse = Mathf.InverseLerp(0, delayDuration, delayTimer);
                // if (stockSpawnObject)
                // {
                //     stockSpawnObject.transform.localScale = Vector3.Lerp(initScale, Vector3.one*1.2f, inverse);
                // }
               
                
                if (_movingObjectCount>=MaxSpawnCount)
                    break;
                
                yield return null;
            }

            if (stockSpawnObject)
            {
                stockSpawnObject.Rb.isKinematic = true;
                stockSpawnObject.Col.enabled = false;
            }
          
            stockSpawnObject.transform.SetParent(collectTuple.Item2);
            var initWorldPos =  stockSpawnObject.transform.position;
            var initScale =  stockSpawnObject.transform.localScale;
            var moveTimer = 0f;
            isChecked = false;
            var targetPos = collectTuple.Item1;

            var initPos = Vector3.zero;
            if (stockSpawnObject)
            {
                initPos =  stockSpawnObject.transform.localPosition;
                initScale =  stockSpawnObject.transform.localScale;
            }
           
            var duration =Random.Range(0.25f,0.5f);

            while (!isChecked)
            {
                moveTimer += Time.deltaTime;
                if (moveTimer > duration)
                {
                    moveTimer = duration;
                    isChecked = true;
                }

                var t = EaseHelper.EaseInSine(moveTimer, duration);
                if (stockSpawnObject)
                {
                    stockSpawnObject.transform.localPosition = NMath.SmoothCurveY(initPos, targetPos, 1f, t);
                    stockSpawnObject.transform.localScale = Vector3.Lerp(initScale, downScaleRate * Vector3.one, t);

                }
               
                if (_movingObjectCount>=MaxSpawnCount)
                    break;
                
                yield return null;
            }
            _movingObjectCount--;
            if (_movingObjectCount<=0)
                _movingObjectCount = 0;
           
            onFinishedAction?.Invoke();
        }
        private IEnumerator Move3DToUIRoutineWitchPhysicCustom(ICustomStock customStock,Transform targetRoot,Action onFinishedAction = null,float downScaleRate = 0.5f)
        {
            yield return null;
            
            var customStockTransform = customStock.GetTransform();
            customStockTransform.SetParent(CameraTransform);
            
            
            var initScale =  customStockTransform.localScale;
            //_movingObjectCount++;
            
            var delayMin = 0f;
            var delayMax = 0.02f;
            var delayDuration = Random.Range(delayMin,delayMax);
            bool isChecked = false;
            var delayTimer = 0f;
            var rb = customStock.GetRigidBody();
            var col = customStock.GetCollider();
            if (customStockTransform)
            {
                if (rb)
                    rb.isKinematic = true;

                if (col)
                    col.enabled = false;
               
            }

            while (!isChecked)
            {
                delayTimer += Time.deltaTime;
                
                if (delayTimer > delayDuration)
                {
                    delayTimer = delayDuration;
                    isChecked = true;
                }
                
                var inverse = Mathf.InverseLerp(0, delayDuration, delayTimer);
                if (!customStockTransform)
                    yield break;
                //stockSpawnObject.transform.localScale = Vector3.Lerp(initScale, initScale*1.2f, inverse);
                
                // if (_movingObjectCount>=MaxSpawnCount)
                //     break;
                //
                yield return null;
            }

           
            
            var moveTimer = 0f;
            isChecked = false;
          
            if (customStockTransform)
            {
                initScale =  customStockTransform.localScale;
            }
          
            var duration =Random.Range(0.5f,1f);
            var lastPos = Get3DMovePos(targetRoot).Item1;
            var initPos = customStockTransform.position;
            while (!isChecked)
            {
                moveTimer += Time.deltaTime;
                if (moveTimer > duration)
                {
                    moveTimer = duration;
                    isChecked = true;
                }
                lastPos =Get3DMovePos(targetRoot).Item1;
                var t = EaseHelper.EaseInSine(moveTimer, duration);
                if (!customStockTransform)
                    yield break;
                customStockTransform.localPosition = NMath.SmoothCurveY(initPos, lastPos, 1f, t);
                customStockTransform.localScale = Vector3.Lerp(initScale, downScaleRate * initScale, t);
                yield return null;
            }
            
            if (customStockTransform)
                customStock.ActivateDisable();
           
            onFinishedAction?.Invoke();
        }

        #endregion

        #region Editor
#if UNITY_EDITOR
        [Button,TabGroup("Tabs","Examples")]
        private void SpawnStockEditor(StockTypes stockType,int count = 3,bool useRandomTypes = false)
        {
            for (int i = 0; i < count; i++)
            {
                var targetType = stockType;
                if (useRandomTypes)
                {
                    var rnd = Enum.GetNames(typeof(StockTypes));
                    targetType = (StockTypes)Random.Range(0, rnd.Length);
                }
                StockStatic.SpawnStock(targetType,1);
            }
        }
        
        [Button,TabGroup("Tabs","Examples")]
        private void SpawnStockEditorVector(StockTypes stockType,Vector3 startVector,int count = 3,bool useRandomTypes = false)
        {
            for (int i = 0; i < count; i++)
            {
                var targetType = stockType;
                if (useRandomTypes)
                {
                    var rnd = Enum.GetNames(typeof(StockTypes));
                    targetType = (StockTypes)Random.Range(0, rnd.Length);
                }
                StockStatic.SpawnStock(targetType,startVector,1);
            }
        }
        
        [Button,TabGroup("Tabs","Examples")]
        private void SpawnStockEditor3D(StockTypes stockType,int count = 3,bool useRandomTypes = false)
        {
            for (int i = 0; i < count; i++)
            {
                var targetType = stockType;
                if (useRandomTypes)
                {
                    var rnd = Enum.GetNames(typeof(StockTypes));
                    targetType = (StockTypes)Random.Range(0, rnd.Length);
                }
                StockStatic.SpawnStock3D(targetType,1);
            }
        }
        
        [Button,TabGroup("Tabs","Examples")]
        private void SpawnStockEditorVector3D(StockTypes stockType,Vector3 startVector,int count = 3,bool useRandomTypes = false)
        {
            for (int i = 0; i < count; i++)
            {
                var targetType = stockType;
                if (useRandomTypes)
                {
                    var rnd = Enum.GetNames(typeof(StockTypes));
                    targetType = (StockTypes)Random.Range(0, rnd.Length);
                }
                StockStatic.SpawnStock3D(targetType,startVector,1);
            }
        }
#endif
        #endregion
    }
}
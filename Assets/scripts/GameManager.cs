using System.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine.UI;
using Unity.Transforms;

public class GameManager : MonoBehaviour
{
    public static GameManager instanse;

    public GameObject ballPrefab; //шарик
    public GameObject capsulePrefab; //капсулы
    //public GameObject plane;
    public Text scoreText;
    public Text existText;

    private int curScore;
    private int curExist;
    private Entity ballEntityPrefab; //MoveComponent
    private Entity capsuleEntityPrefab; //RotateComponent
    //private Entity planeEntity; //Поверхность
    private EntityManager manager;
    private BlobAssetStore blobAssetStore;

    private void Awake()
    {
        if (instanse != null && instanse != this)
        {
            Destroy(gameObject); //зачем дестроить?
            return;
        }

        instanse = this;

        manager = World.DefaultGameObjectInjectionWorld.EntityManager; //чтобы настраивать энтити
        blobAssetStore = new BlobAssetStore();
        //Чтобы спавнить новые энтити
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
        ballEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(ballPrefab, settings);
        capsuleEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(capsulePrefab, settings);        
    }

    private void OnDestroy()
    {
        blobAssetStore.Dispose(); //почистить память от предыдущих инстансов        
    }

    private void Start()
    {
        curScore = 0;
        curExist = 1;
        DisplayInfo();
        SpawnBall();        
    }

    private void Update()
    {        
        //Нужно как-то завязаться на количество существующих entity Capsule
        if (Input.GetKeyDown(KeyCode.G))
        {            
            curExist++;
            DisplayInfo();
            NewCapsule();
        }                       
    }

    public void IncreaseScore()
    {
        curScore++;
        curExist--;
        DisplayInfo();
    }

    private void DisplayInfo()
    {
        scoreText.text = "Score: " + curScore;
        existText.text = "Exist: " + curExist;
    }

    void SpawnBall()
    {
        Entity newBallEntity = manager.Instantiate(ballEntityPrefab);

        //позиция для спавна мяча
        Translation ballTrans = new Translation
        {
            Value = new float3(0f, 5f, -5f)
        };

        manager.AddComponentData(newBallEntity, ballTrans); //Присвоить позицию нашему новому энтити
        CameraFollow.instanse.ballEntity = newBallEntity; //ссылка на энтити мяча для камеры
    }

    public void NewCapsule()
    {        
        Entity newCapsuleEntity = manager.Instantiate(capsuleEntityPrefab);
        //Translation planePos = manager.GetComponentData<Translation>(planeEntity);

        Translation capsuleTrans = new Translation
        {
            Value = new float3(
                UnityEngine.Random.Range(-45, 45),
                2f,
                UnityEngine.Random.Range(-45, 45))            
        };
        manager.AddComponentData(newCapsuleEntity, capsuleTrans);
    }
}

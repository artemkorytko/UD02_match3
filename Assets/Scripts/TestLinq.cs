using System;
using UnityEngine;
using System.Linq;

namespace DefaultNamespace
{
    public class TestLinq : MonoBehaviour
    {
        private Class2[] classes;

        private void Awake()
        {
            classes = new Class2[10];
            for (int i = 0; i < 10; i++)
            {
                var class2 = new Class2();
                class2.class1 = new Class1();
                class2.class1.value = i;
                class2.class1.id = $"id {i}";
                class2.class1.position = new Vector3(i, i, i);
                classes[i] = class2;
            }
        }

        private void Start()
        {
            var firstElement = classes.FirstOrDefault(); //classes[0];
            var firstElement1 = classes.FirstOrDefault(item => item.class1.id == "4" && item.class1.value == 4);

            var sortArray = classes.OrderBy(x => Vector3.SqrMagnitude(x.class1.position - transform.position)).ToDictionary(x => x.class1.id);
            var sortArray1 = classes.Where(x => x.class1.position.x < 5).ToArray();

            var newArray = classes.Select(x => x.class1.position).ToArray();

            var id = classes.Where(x => x.class1.value < 5).Select(x => x.class1.id).OrderByDescending(x => x.Length).FirstOrDefault();
        }
    }

    [Serializable]
    public class Class1
    {
        [SerializeField] private Transform newValue;
        
        public Vector3 position;
        public string id;
        public int value;

        public Transform NewValue => newValue;
    }

    [Serializable]
    public class Class2
    {
        public Class1 class1;
    }
}
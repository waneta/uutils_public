using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Client.UUtils.Example
{
    public class Example_EncryptAndCompress_AES : MonoBehaviour
    {

        private void OnGUI()
        {
            
            if (GUILayout.Button("比较两个字节内容是否相同"))
            {
                byte[] array1 = new byte[] { 1, 2, 3, 4, 5 };
                byte[] array2 = new byte[] { 1, 2, 3, 4, 5 };
                byte[] array3 = new byte[] { 5, 4, 3, 2, 1 };

                bool areArraysEqual = array1.SequenceEqual(array2);
                Debug.LogError($"Array1 and Array2 are equal: {areArraysEqual}");

                areArraysEqual = array1.SequenceEqual(array3);
                Debug.LogError($"Array1 and Array3 are equal: {areArraysEqual}");
            }

            if (GUILayout.Button("FileManager iv 和key"))
            {
                //获取FileManager缺省
                byte[] key = FileManager.Instance.getKey();
                byte[] iv = FileManager.Instance.getIV();

                Debug.LogErrorFormat("key:{0} key:{1}", key.Length, BitConverter.ToString(key));
                Debug.LogErrorFormat("key:{0} iv:{1}", iv.Length, BitConverter.ToString(iv));

                //测试方法
                EncryptionUtils EncryptionUtil = new EncryptionUtils();

                byte[] test_key = EncryptionUtil.getKey();
                byte[] test_iv = EncryptionUtil.getIV();
                Debug.LogErrorFormat("key:{0} key:{1}", test_key.Length, BitConverter.ToString(test_key));
                Debug.LogErrorFormat("key:{0} iv:{1}", test_iv.Length, BitConverter.ToString(test_iv));


                //比较
                bool areArraysEqual = key.SequenceEqual(test_key);
                Debug.LogError($"key and test_key are equal: {areArraysEqual}");

                areArraysEqual = iv.SequenceEqual(test_iv);
                Debug.LogError($"iv and test_iv are equal: {areArraysEqual}");


            }
            if (GUILayout.Button("生成64 base"))
            {
                int keyBaseLength = 64;
                System.Random random = new System.Random();
                for (int i = 0; i < 5; i++)
                {
                    string keyBase = EncryptionUtils.GenerateRandomKeyBase(keyBaseLength, random);
                    Debug.LogError($"{keyBase}");
                }
                
            }
            if(GUILayout.Button("设置FileManager密钥对"))
            {
                int keyBaseLength = 64;
                System.Random random = new System.Random();
                string[] array = new string[5];

                for (int i = 0; i < 5; i++)
                {
                    string keyBase = EncryptionUtils.GenerateRandomKeyBase(keyBaseLength, random);
                    Debug.LogError($"{keyBase}");
                    array[i] = keyBase;
                }

                FileEncryptConfig.SetEncryptBase64(array[0], array[1], array[2], array[3], array[4]);
            }


        }

    }
}

using UnityEngine;

namespace Root.Scripts.MainScene.IslandStarter
{
    public class Test : MonoBehaviour
    {
        public float speed = 5f; // Hareket hızı
        public float rotationSpeed = 200f; // Dönme hızı

        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>(); // Rigidbody bileşenini al
        }

        private void FixedUpdate()
        {
            // Kullanıcıdan giriş al
            float moveInput = Input.GetAxis("Vertical"); // W/S veya Yukarı/Aşağı tuşları
            float turnInput = Input.GetAxis("Horizontal"); // A/D veya Sol/Sağ tuşları

            // İleri veya geri hareket
            Vector3 moveDirection = transform.forward * moveInput * speed;
            rb.AddForce(moveDirection, ForceMode.Force);

            // Dönüş
            float turnAmount = turnInput * rotationSpeed * Time.fixedDeltaTime;
            Quaternion turnRotation = Quaternion.Euler(0, turnAmount, 0);
            rb.MoveRotation(rb.rotation * turnRotation);
        }
    }
}
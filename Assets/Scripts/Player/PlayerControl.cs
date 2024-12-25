using System.Collections;
using TS.Weapon;
using UnityEngine;

namespace TS.Player {

    public class PlayerControl : MonoBehaviour {
        public Animator animator;
        public WeaponController weaponController;

        private void Start() {
            animator = GetComponent<Animator>();
            weaponController = GetComponentInChildren<WeaponController>();
        }

        private void Update() {

            if (Input.GetMouseButton(0)) {
                weaponController.TryAttack();
            }
            //var aimPoint = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            var aimPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition); //可以直接用Camera.main获取主相机
            weaponController.Aim(aimPoint);
        }

        public void IsDie()
        {
            UIManager.Instance.ShowGameOver(true);
            GameManager.Instance.ChangeGameState(GameState.GameOver);
        }

        public void StartBlink(Material mat)
        {
            StartCoroutine(Blink(mat));
        }
        private IEnumerator Blink(Material mat)
        {
            for (int i = 0; i < 4; i++)
            {
                mat.SetColor("_Color", i % 2 == 0 ? Color.red : Color.white );
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
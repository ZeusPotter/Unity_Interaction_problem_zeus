using System;
using UnityEngine;

namespace RayCaster.Utils
{
    public class EntradaVR : MonoBehaviour
    {
       
        public enum SwipeDirection
        {
            NINGUNA,
            ARRIBA,
            ABAJO,
            IZQUIERDA,
            DERECHA
        };


        public event Action<SwipeDirection> AlDeslizar;             
        public event Action AlHacerClick;                           
        public event Action HaciaAbajo;                             
        public event Action HaciaArriba;                            
        public event Action AlHacerDobleClick;                      
        public event Action AlCancelar;                             


        public float TiempoDeDobleClick = 0.3f;                     
        public float AnchoDeDeslizamiento = 0.3f;                   

        
        private Vector2 PosicionDelMouseHaciaAbajo;                 
        private Vector2 PosicionDelMouseHaciaArriba;                
        private float UltimaHoraDelMouse;                           
        private float UltimoValorHorizontal;                        
        private float UltimoValorVertical;                          


        public float TiempoDobleClick { get { return TiempoDeDobleClick; } }


        private void Update()
        {
            ComprobarEntrada();
        }


        private void ComprobarEntrada()
        {
            
            SwipeDirection GolpeFuerte = SwipeDirection.NINGUNA;

            if (Input.GetButtonDown("Fire1"))
            {
               
                PosicionDelMouseHaciaAbajo = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            
                
                if (HaciaAbajo != null)
                    HaciaAbajo();
            }

            
            if (Input.GetButtonUp ("Fire1"))
            {
                
                PosicionDelMouseHaciaArriba = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);

                
                GolpeFuerte = DetectarDeslizar ();
            }

            
            if (GolpeFuerte == SwipeDirection.NINGUNA)
                GolpeFuerte = DetectarElGolpeDeTecladoEmulado();

            
            if (AlDeslizar != null)
                AlDeslizar(GolpeFuerte);

            
            if(Input.GetButtonUp ("Fire1"))
            {
               
                if (HaciaArriba != null)
                    HaciaArriba();

                
                
                if (Time.time - UltimaHoraDelMouse < TiempoDeDobleClick)
                {
                    
                    if (AlHacerDobleClick != null)
                        AlHacerDobleClick();
                }
                else
                {
                    
                    if (AlHacerClick != null)
                        AlHacerClick();
                }

               
                UltimaHoraDelMouse = Time.time;
            }

            
            if (Input.GetButtonDown("Cancel"))
            {
                if (AlCancelar != null)
                    AlCancelar();
            }
        }

        private SwipeDirection DetectarDeslizar ()
        {
            
            Vector2 DeslizarLosDatos = (PosicionDelMouseHaciaArriba - PosicionDelMouseHaciaAbajo).normalized;

           
            bool ElGolpeEsVertical = Mathf.Abs (DeslizarLosDatos.x) < AnchoDeDeslizamiento;

            
            bool ElGolpeEsHorizontal = Mathf.Abs(DeslizarLosDatos.y) < AnchoDeDeslizamiento;

            
            if (DeslizarLosDatos.y > 0f && ElGolpeEsVertical)
                return SwipeDirection.ARRIBA;

            
            if (DeslizarLosDatos.y < 0f && ElGolpeEsVertical)
                return SwipeDirection.ABAJO;

           
            if (DeslizarLosDatos.x > 0f && ElGolpeEsHorizontal)
                return SwipeDirection.DERECHA;

            
            if (DeslizarLosDatos.x < 0f && ElGolpeEsHorizontal)
                return SwipeDirection.IZQUIERDA;

            
            return SwipeDirection.NINGUNA;
        }


        private SwipeDirection DetectarElGolpeDeTecladoEmulado()
        {
            
            float Horizontal = Input.GetAxis ("Horizontal");
            float Vertical = Input.GetAxis ("Vertical");

            
            bool NoEntradaHorizontalPreviamente = Mathf.Abs (UltimoValorHorizontal) < float.Epsilon;
            bool NoEntradaVerticalPreviamente = Mathf.Abs(UltimoValorVertical) < float.Epsilon;

            
            UltimoValorHorizontal = Horizontal;
            UltimoValorVertical = Vertical;

            
            if (Vertical > 0f && NoEntradaVerticalPreviamente)
                return SwipeDirection.ARRIBA;

            
            if (Vertical < 0f && NoEntradaVerticalPreviamente)
                return SwipeDirection.ABAJO;

            
            if (Horizontal > 0f && NoEntradaHorizontalPreviamente)
                return SwipeDirection.DERECHA;

            
            if (Horizontal < 0f && NoEntradaHorizontalPreviamente)
                return SwipeDirection.IZQUIERDA;

            
            return SwipeDirection.NINGUNA;
        }
        

        private void OnDestroy()
        {
            
            AlDeslizar = null;
            AlHacerClick = null;
            AlHacerDobleClick = null;
            HaciaAbajo = null;
            HaciaArriba = null;
        }
    }
}
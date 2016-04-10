using UnityEngine;
using System.Collections;

public class TubosHardScript : MonoBehaviour
{

    //declaramos la velocidad inicial de la columna
    public Vector3 velocidad;
    //La distancia que habra entre una columna y otra
    public Vector3 distanciaEntreColumnas;
    //La forma correcta de hacerlo ¿?
    public SpriteRenderer formaColumna;
    //Sumamos una vez
    private bool sumarPuntaje = true;
    public GUIText Puntajes;
    public bool juegoIniciado;
    //sonido de cada punto
    public AudioClip sonidoPunto;

    void Update()
    {
        //funcion que mueve los tubos
        if (juegoIniciado == true)
            moverTubo();
    }


    private void moverTubo()
    {
        //Los tubos iran avanzando de a pocos, igual que el Flappy bird
        this.transform.position = this.transform.position + (velocidad * Time.deltaTime);

        //if(formaColumna.isVisible == true)

        if (this.transform.position.x <= -4f)
        {
            //Le aumentamos la distancia entre columnas al llegar a la posicion -13.5
            Vector3 posicionTemporal = this.transform.position + distanciaEntreColumnas;
            //Cambiamos el lugar en Y por uno random
            posicionTemporal.y = Random.Range(-2.33f, 2.63f);
            //Movemos a los tubos a esa posicion
            this.transform.position = posicionTemporal;
            sumarPuntaje = true;
        }
        //Sumamos una vez cuando los tubos pasan al pajaro
        if (this.transform.position.x <= -2.53 & sumarPuntaje == true)
        {
            sumarPuntaje = false;
            int puntos = int.Parse(Puntajes.text) + 1;
            Puntajes.text = puntos.ToString();
            //llamamos al sonido
            ReproducirSonido(sonidoPunto);
        }
    }

    //Reproduce un efecto de sonido
    private void ReproducirSonido(AudioClip clipOriginal)
    {
        // Como no es un sonido 3D la posicion no importa
        AudioSource.PlayClipAtPoint(clipOriginal, transform.position);
    }
}

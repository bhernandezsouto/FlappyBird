using UnityEngine;
using System.Collections;

public class BirdScript : MonoBehaviour
{
    //Declaramos la velocidad inicial del pajaro sea igual a zero, Vector3.zero = 0,0,0
    //1,1,0
    Vector3 velocidad = Vector3.zero;
    //Declaramos un vector que controle la gravedad, no usaremos la fisica de unity
    public Vector3 gravedad;
    //Declaramos un vector que define el salto (aleteo) del pajaro
    public Vector3 velocidadAleteo;
    //Declaramos si se debe aletear, si se toco la pantalla o se presiono espacio
    bool aleteo = false;
    //Declaramos la velocidad maxima de rotacion del pajaro
    public float velocidadMaxima;

    //Agregamos una referencia de los tubos en flappy para poder modificarlos
    public TubosScript tubo1;
    public TubosScript tubo2;
    private bool juegoTerminado;
    private Animator anim;
    private bool juegoIniciado;
    public AudioClip sonidoPunto;
    public GameObject gameover;
    public int chocar;
    public RectTransform botones;
    public GUIText Puntajes;
    public GUIText PMaximo;
    public GUIText PObtenido;
    public GameObject CuadroPuntaje;
    public GameObject Principiante;
    public GameObject Bronce;
    public GameObject Plata;
    public GameObject Oro;

    // Use this for initialization
    void Start()
    {
        Cargar();
        chocar = 0;
        anim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //aumenta con el numero de presiones en la pantalla
        int numPresiones = 0;
        foreach (Touch toque in Input.touches)
        {
            if (toque.phase != TouchPhase.Ended && toque.phase != TouchPhase.Canceled)
                numPresiones++;
        }
        //Si la persona presiona el boton de espacio o hace clic en la pantalla con el mouse, o tocas con el dedo
        if (Input.GetKeyDown(KeyCode.Space) | Input.GetMouseButtonDown(0) | numPresiones > 0)
        {
            if (juegoTerminado == false)
                aleteo = true;
            juegoIniciado = true;
            tubo1.juegoIniciado = true;
            tubo2.juegoIniciado = true;
        }
    }

    //Este es el update de la fisica, que es ligeramente mas lento que el update del juego
    void FixedUpdate()
    {
        if (juegoIniciado)
        {
            //A la velocidad le sumamos la gravedad (Para que el pajaro caiga)
            velocidad += gravedad * Time.deltaTime;

            //Si presionaron espacio o hicieron clic
            if (aleteo == true)
            {
                //Que solo sea una vez
                aleteo = false;
                //El vector velocidad recibe el impulso hacia arriba al pajaro
                velocidad.y = velocidadAleteo.y;
            }
            //Hacemos que el pajaro reciba la velocidad (la gravedad lo hace caer mas rapido)
            transform.position += velocidad * Time.deltaTime;
            float angulo = 0;
            if (velocidad.y >= 0)
            {
                //Cambiamos el angulo si Y es positivo que mire arriba
                angulo = Mathf.Lerp(0, 25, velocidad.y / velocidadMaxima);
            }
            else {
                //Cambiamos el angulo si Y es negativo que mire abajo
                angulo = Mathf.Lerp(0, -75, -velocidad.y / velocidadMaxima);
            }
            //Rotamos
            transform.rotation = Quaternion.Euler(0, 0, angulo);
        }
    }
    //Cada vez que haya una colision con cualquier objeto que tenga un collider se actiavara esta funcion
    //Collider son Box Collider 2D, Circle Collider 2D, etc.
    void OnCollisionEnter2D(Collision2D colision)
    //void OnTriggerEnter2D(Collider2D colision)
    {
        //Si colisionamos con el tubo que se detengan los tubos
        if (colision.gameObject.name == "TuboInferior" | colision.gameObject.name == "TuboSuperior" | colision.gameObject.name == "Piso")
        {
            //Hacemos que la velocidad de los tubos se haga cero
            tubo1.velocidad = new Vector3(0, 0, 0);
            tubo2.velocidad = new Vector3(0, 0, 0);
            //Dejamos de ejecutar el aleteo(impulso) al hacer clic
            juegoTerminado = true;
            anim.SetTrigger("JuegoTerminado");
            gameover.SetActive(true);
            chocar = chocar + 1;
        }
        //Al momento de caer, queremos ignorar la colision con el tubo de abajo
        if (colision.gameObject.name == "TuboInferior")
        {
            colision.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        }
        //Si colisionamos con el Piso, que la gravedad no siga aumentando
        if (colision.gameObject.name == "Piso")
        {
            gravedad = new Vector3(0, 0, 0);
        }
        if (juegoTerminado == true & chocar == 1)
        {
            MostrarBotones();
            MostrarCuadro();
            ReproducirSonido(sonidoPunto);
            int puntosconseguidos = int.Parse(Puntajes.text);
            int puntosmaximos = int.Parse(PMaximo.text);
            if (puntosconseguidos > puntosmaximos)
            {
                PMaximo.text = Puntajes.text;
                Guardar(Puntajes.text);
            }
            Puntajes.gameObject.SetActive(false);
            PObtenido.text = Puntajes.text;
            MostrarMedallas();
        }
    }
    public void MostrarMedallas()
    {   
        int puntos = int.Parse(PMaximo.text);
        if (puntos >=10 & puntos < 20)
        {
            Principiante.gameObject.SetActive(true);
        }
        if (puntos >= 20 & puntos < 50)
        {
            Bronce.gameObject.SetActive(true);
        }
        if (puntos >= 50 & puntos < 70)
        {
            Plata.gameObject.SetActive(true);
        }
        if (puntos >= 70)
        {
            Oro.gameObject.SetActive(true);
        }
    }
    //Muestra los botones
    private void MostrarBotones()
    {
        botones.gameObject.SetActive(true);
    }
    private void MostrarCuadro()
    {
        CuadroPuntaje.gameObject.SetActive(true);
    }
    //Reproduce un efecto de sonido
    private void ReproducirSonido(AudioClip clipOriginal)
    {
        // Como no es un sonido 3D la posicion no importa
        AudioSource.PlayClipAtPoint(clipOriginal, transform.position);
    }
    public void Guardar(string puntaje)
    {
        PlayerPrefs.SetString("Puntaje", puntaje);
    }
    public void Cargar()
    {
        PMaximo.text = PlayerPrefs.GetString("Puntaje","0");
    }
}

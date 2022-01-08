using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class Generador : MonoBehaviour
{
    public List<Sprite> _listImagen;
    public List<Image> _listImagenUI;
    public Image _imgSeleccion;
    public int _seleccion;
    public List<int> _listEscogidos;

    public Sprite _imgIncongita;

    private int _contador;
    private bool _interactuar;

    private int _score;
    public TextMeshProUGUI _txtScore;

    private int _scoreViejo;
    public TextMeshProUGUI _txtScoreViejo;

    public AudioSource _sndGanar, _sndPerder;

    void Start()
    {
        initJuego();

        _scoreViejo = PlayerPrefs.GetInt("score",0);
        _txtScoreViejo.text = "Max  Score : " + _scoreViejo;
    }

    void initJuego()
    {
        _listEscogidos.Clear();
        _seleccion = 0;
        _contador = 5;



        calcularRandom(3);

        int i = 0;
        _listImagenUI.ForEach(img=>{
            img.sprite = _listImagen[_listEscogidos[i]];
            i++;
        });

        _seleccion = _listEscogidos[Random.Range(0,3)];
        
        _imgSeleccion.sprite = _listImagen[_seleccion];


        _interactuar = false;
        
        
        StartCoroutine(secuenciaCarta());

    }

    void calcularRandom(int i)
    {
        if(i!=0)
        {
            var e = Random.Range(0,30);
            var val = false;

            _listEscogidos.ForEach(x=>{
                if(e == x)
                {
                    val = true;
                    
                    return;
                    
                }

            });

            if(val)
            {
                calcularRandom(i);
            }
            else 
            {
                _listEscogidos.Add(e);
                calcularRandom(i-1);
            }
        }
    }

    IEnumerator secuenciaCarta()
    {
        yield return new WaitForSeconds(0.25f);

        while (_contador>1)
        {
            mostrar();
            _contador --;
            yield return new WaitForSeconds(0.25f);
        
        }

        
        yield return new WaitForSeconds(0.25f);

        _interactuar=true;
        mostrarNinguno();
    }

    void mostrar()
    {
        _listEscogidos.Shuffle();

        mostrarCartas();
    }

    void mostrarCartas()
    {
        var listaPos = new List<int>{0,1,2};
        listaPos.Shuffle();

        var elegido = Random.Range(0,3);
        var elegido2 = Random.Range(0,2);

        for(int i = 0; i<listaPos.Count; i++)
        {
            if(elegido2 == 1)
            {
                if(i==elegido)
                {
                    cambiarCartaNinguna( listaPos[i]);
                }
                else 
                {
                    cambiarCarta( listaPos[i]);
                }
            }
            else 
            {
                if(i!=elegido)
                {
                    cambiarCartaNinguna( listaPos[i]);
                }
                else 
                {
                    cambiarCarta( listaPos[i]);
                }
            }   

            cambiarCarta( listaPos[i]);
        }
    }

    void cambiarCarta(int r)
    {
        var img = _listImagenUI[r];
        img.sprite = _listImagen[_listEscogidos[r]];
    }

    void cambiarCartaNinguna(int r)
    {
        var img = _listImagenUI[r];
        img.sprite = _imgIncongita;
    }

    void mostrarNinguno()
    {
         for(int i = 0; i<_listEscogidos.Count; i++)
        {
            cambiarCartaNinguna(i);
        }
    }


    public void checkCarta(int i)
    {
        if(_interactuar)
        {
            if(_listEscogidos[i] == _seleccion)
            {
                _sndGanar.Play();
                _score++;
                editarScore();
                guardarScore();
            }
            else 
            {
                _sndPerder.Play();
                _score = 0;
                editarScore();
            }

            initJuego();
        }

    }

    void guardarScore()
    {
        if(_score > _scoreViejo)
        {
            _scoreViejo = _score;
            PlayerPrefs.SetInt("score",_score);

            _txtScoreViejo.text = "Max  Score : " + _scoreViejo;
        }
    }

    void editarScore()
    {
        _txtScore.text = "Score : " + _score;
    }
}

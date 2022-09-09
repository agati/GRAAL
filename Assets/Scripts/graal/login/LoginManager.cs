
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LoginManager : MonoBehaviour
{


    [SerializeField]
    public InputField usuarioField = null;
    [SerializeField]
    public InputField senhaField = null;
    [SerializeField]
    public Text feedbackMsg = null;
    [SerializeField]
    public Toggle rememberData = null;

    public Usuario usuario;
    

    void Start()
    {
        

        if (PlayerPrefs.HasKey("lembra") && PlayerPrefs.GetInt("lembra") == 1)
        {
            usuarioField.text = PlayerPrefs.GetString("lembraUsuario");
            senhaField.text = PlayerPrefs.GetString("lembraSenha");

            usuario = FindObjectOfType<Usuario>();
        }


    }

    public void FazerLogin()
    {
        StopAllCoroutines();

        string usuarioInput = usuarioField.text;
        string senhaInput = senhaField.text;
        Debug.Log("usuario vale:" + usuarioInput + " e senha vale:" + senhaInput);

        if (usuarioInput == "" || senhaInput == "")
        {
            FeedbackError("Preencha todos os campos");
        }
        else
        {

            if (rememberData.isOn)
            {
                PlayerPrefs.SetInt("lembra", 1);
                PlayerPrefs.SetString("lembraUsuario", usuarioInput);
                PlayerPrefs.SetString("lembraSenha", senhaInput);
            }
            //para uso com o XAMPP
            //string url="http://127.0.0.1/graal-unity/login.php"+"?login="+usuarioInput+"&senha="+senhaInput;

            //para uso com o SpringBoot
            string url = "http://127.0.0.1:8080/usuarios/autenticar" + "?login=" + usuarioInput + "&senha=" + senhaInput;

            Debug.Log("url completa:" + url);

            usuario.login = usuarioInput;
            usuario.senha = senhaInput;

            Debug.Log("Iniciar ValidaLogin()...");

            StartCoroutine(ValidaLogin(url, usuarioInput, senhaInput));
            
            if(usuario.login!="" && usuario.senha != "")
            {
                //login e senha corretos . Carrega os dados completos do usuário 
                StopAllCoroutines();
                StartCoroutine(GetUsuario(usuario.login, usuario.senha));
                               
            }
                     

        }

    }//fim do metodo FazerLogin()  *******************************

    
   public IEnumerator ValidaLogin(string urlString, string usuarioLoginString, string senhaLoginString)
    {

        Debug.Log("...iniciando a rotina valida login...");
        Debug.Log("url: " + urlString + ", login: " + usuarioLoginString + ", senha: " + senhaLoginString);

        
        WWW www2 = new WWW(urlString);
        yield return new WaitUntil(() => www2.isDone);

        if (www2.isDone)
        {

            Debug.Log("o site retornou :" + www2.text);

            if (www2.text == "1")
            {
                //logou corretamente
                usuario.login = usuarioLoginString;
                usuario.senha = senhaLoginString;

                Debug.Log("Login realizado com sucesso!");
                FeedbackOk("Login realizado com sucesso!\nCarregando o jogo...");
                
            }


            if (www2.text == "0")
            {//não achou usuário
                Debug.Log("Usuário ou senha não encontrados!");
                FeedbackError("Usuario ou senha não encontrados!");
                usuario.login = "";
                usuario.senha = "";
            }

            if (www2.text == "2")
            {//Mais que um resultado encontrado!
                Debug.Log("Mais que um resultado encontrado!");
                FeedbackError("Mais que um resultado encontrado!");
                usuario.login = "";
                usuario.senha = "";

            }

            if (www2.text == "3")
            {//Usuário não encontrado para o login informado!
                Debug.Log("Usuário não encontrado para o login informado!");
                FeedbackError("Usuário não encontrado para o login informado!");
                usuario.login = "";
                usuario.senha = "";

            }

        }
        else
        {
            //Qualquer outra situação
            Debug.Log("Ocorreu uma situação não esperada!");
            FeedbackError("Ocorreu uma situação não esperada!");
            usuario.login = "";
            usuario.senha = "";
        }

        
    }//*********** fim do ValidaLogin() **************************


    public IEnumerator GetUsuario(string login, string senha)
    {
        string url = "http://127.0.0.1:8080/usuarios/por-login" + "?login=" + login + "&senha=" + senha; ;

        Debug.Log("GetUsuario(): A url de acesso ao servidor vale: " + url);

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("GetUsuario(): Servidor retornou: " + www.error);
            usuario.login = "";
            usuario.senha = "";
            FeedbackError("Usuário não encontrado!");
        }
        else
        {
            // deserializa o json e salva no usuario
            Debug.Log("GetUsuario(): O servidor retornou: " + www.downloadHandler.text);

            string jsonString = www.downloadHandler.text.ToString();
            JsonUtility.FromJsonOverwrite(jsonString, usuario);

            Debug.Log("GetUsuario(): O Json do usuário recebido convertido para objeto tem usuário id: " + usuario.id);
            FeedbackError("Usuário encontrado!");
            CarregaCena();
        }

    }//*********** fim do GetUsuario() **************************



    public void RegistraUsuario()
    {
        Debug.Log("...iniciando a rotina RegistraUsuario()...");

        StopAllCoroutines();

        string usuario = usuarioField.text;
        string senha = senhaField.text;
        Debug.Log("usuario vale:" + usuario + " e senha vale:" + senha);
        if (usuario == "" || senha == "")
        {
            FeedbackError("Preencha todos os campos");
        }
        //para uso com o xampp
        //string url="http://127.0.0.1/graal-unity/insere.php?"+"login="+usuario+"&senha="+senha;

        //para uso com o SpringBoot
        string url = "http://127.0.0.1:8080/usuarios/registrar" + "?login=" + usuario + "&senha=" + senha;

        Debug.Log("url completa:" + url);

        Debug.Log("acessando o site...");

        StartCoroutine(InsereUsuario(url));

        Debug.Log("rodou InsereUsuario...");

    }////*********** fim do RegistraUsuario() **************************

    IEnumerator InsereUsuario(string url)
    {

        Debug.Log("...iniciando a rotina InsereUsuario()...");

        WWW www = new WWW(url);
        yield return new WaitUntil(() => www.isDone);

        if (www.isDone) Debug.Log("o site retornou :" + www.text);

        if (www.text == "2")
        {//já existe um usuário com esse login
            Debug.Log("Já existe um usuário com esse login!");
            FeedbackError("Já existe um usuário com esse login!");

        }

        if (www.text == "1")
        {//gravou corretamente
            Debug.Log("Registro realizado com sucesso!");
            FeedbackOk("Registro realizado com sucesso!");
            //pega todos os campos do usuário

        }

        if (www.text == "0")
        {//não registrou usuário
            Debug.Log("Usuário não foi registrado!");
            FeedbackError("Usuario não foi registrado!");
        }

    }//fim do InsereUsuario() ********************************

    void CarregaCena()
    {
        Debug.Log("...iniciando a rotina CarregaCena()...");
        SceneManager.LoadScene("MenuInicial");

    }//fim do CarregaCena()  **************************



    void FeedbackOk(string mensagem)
    {

        feedbackMsg.CrossFadeAlpha(100f, 0f, false);
        feedbackMsg.color = Color.green;
        feedbackMsg.text = mensagem;
        feedbackMsg.CrossFadeAlpha(0f, 2f, false);
        usuarioField.text = "";
        senhaField.text = "";



    }//fim do método FeedbackOk *****************

    void FeedbackError(string mensagem)
    {

        feedbackMsg.CrossFadeAlpha(100f, 0f, false);
        feedbackMsg.color = Color.green;
        feedbackMsg.text = mensagem;
        feedbackMsg.CrossFadeAlpha(0f, 2f, false);
        usuarioField.text = "";
        senhaField.text = "";

    }// fim do método FeedbackError  *****************



}//fim da classe LoginManager


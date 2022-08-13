
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager_OK : MonoBehaviour {


	[SerializeField]
	public InputField usuarioField=null;
	[SerializeField]
	public InputField senhaField=null;
	[SerializeField]
	public Text feedbackMsg=null;
	[SerializeField]
	public Toggle rememberData=null;


	void Start () {


		if (PlayerPrefs.HasKey("lembra") && PlayerPrefs.GetInt("lembra")==1){
			usuarioField.text=PlayerPrefs.GetString("lembraUsuario");
			senhaField.text=PlayerPrefs.GetString("lembraSenha");	
		}


	}

	public void FazerLogin () {

		StopAllCoroutines();

		string usuario=usuarioField.text;
		string senha=senhaField.text;
		Debug.Log("usuario vale:"+usuario+" e senha vale:"+senha);


		if (usuario=="" || senha==""){
			FeedbackError("Preencha todos os campos");

		}else{

			if(rememberData.isOn){
				PlayerPrefs.SetInt("lembra",1);
				PlayerPrefs.SetString("lembraUsuario",usuario);
				PlayerPrefs.SetString("lembraSenha",senha);
			}
			string url="http://127.0.0.1/login.php"+"?login="+usuario+"&senha="+senha;

			Debug.Log ("url completa:"+url);
			Debug.Log("acessando o site...");

			StartCoroutine (ValidaLogin(url));
			Debug.Log("rodou ValidaLogin...");

		}

	}//fim do metodo FazerLogin  *******************************


	IEnumerator ValidaLogin(string url)  {

		Debug.Log("...iniciando a rotina valida login...");


		WWW www=new WWW(url);
		yield return new WaitUntil(()=>www.isDone);

		if (www.isDone)	Debug.Log("o site retornou :"+www.text);

		if(www.text=="1"){//logou corretamente
			Debug.Log("Login realizado com sucesso!\nCarregando o jogo...");
			FeedbackOk("Login realizado com sucesso!\nCarregando o jogo..." );
			CarregaCena();
		}

		if(www.text=="0"){//não achou usuário
			Debug.Log("Usuário ou senha não encontrados!");
			FeedbackError("Usuario ou senha não encontrados!");
		}

		if(www.text=="2"){//Mais que um resultado encontrado!
			Debug.Log("Mais que um resultado encontrado!");
			FeedbackError("Mais que um resultado eencontrado!");

		}

	}//fim do metodo ValidaLogin() ********************************


	public void RegistraUsuario(){

		StopAllCoroutines();

		string usuario=usuarioField.text;
		string senha=senhaField.text;
		Debug.Log("usuario vale:"+usuario+" e senha vale:"+senha);
		if (usuario=="" || senha==""){
			FeedbackError("Preencha todos os campos");
		}

		string url="http://127.0.0.1/insere.php?"+"login="+usuario+"&senha="+senha;
		Debug.Log ("url completa:"+url);

		Debug.Log("acessando o site...");

		StartCoroutine (InsereUsuario(url));

		Debug.Log("rodou InsereUsuario...");

	}

	IEnumerator InsereUsuario(string url) {


		Debug.Log("...iniciando a rotina Insere Usuário...");
		WWW www=new WWW(url);
		yield return new WaitUntil(()=>www.isDone);

		if (www.isDone)	Debug.Log("o site retornou :"+www.text);

		if(www.text=="2"){//já existe um usuário com esse login
			Debug.Log("Já existe um usuário com esse login!");
			FeedbackError("Já existe um usuário com esse login!");

		}

		if(www.text=="1"){//gravou corretamente
			Debug.Log("Registro realizado com sucesso!");
			FeedbackOk("Registro realizado com sucesso!" );

		}

		if(www.text=="0"){//não registrou usuário
			Debug.Log("Usuário não foi registrado!");
			FeedbackError("Usuario não foi registrado!");
		}

	}//fim do metodo InsereUsuario ********************************

	void CarregaCena() {
		SceneManager.LoadScene ("menu");

	}//fim do metodo CarregaCena  **************************



	void FeedbackOk(string mensagem) {

		feedbackMsg.CrossFadeAlpha(100f,0f,false);
		feedbackMsg.color=Color.green;
		feedbackMsg.text=mensagem;
		feedbackMsg.CrossFadeAlpha(0f,2f,false);
		usuarioField.text="";
		senhaField.text="";



	}//fim do método FeedbackOk *****************

	void FeedbackError(string mensagem) {

		feedbackMsg.CrossFadeAlpha(100f, 0f, false);
		feedbackMsg.color=Color.green;
		feedbackMsg.text=mensagem;
		feedbackMsg.CrossFadeAlpha(0f,2f,false);
		usuarioField.text="";
		senhaField.text="";

	}// fim do método FeedbackError  *****************



}//fim da classe LoginManager


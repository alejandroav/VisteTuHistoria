using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class main : MonoBehaviour {
	
	//private RuntimePlatform platform = Application.platform;
	public bool Mode = true;
	public SpriteRenderer Character;
	public SpriteRenderer Dress;
	public Sprite Woman;
	public Sprite Man;	
	public Text text;
	public Scrollbar scrollDress;
	public Scrollbar scrollText;
	public Sprite[] mClothes,fClothes;
	public GameObject container;
	public GameObject itemPrefab;
	public Text message;
	public Animator displayMessage;

	static private int kFSize = 10;
	static private int kMSize = 10;
	private int itemCount, columnCount = 1;
	private int target;
	private string[] fDescriptions = new string[kFSize];
	private string[] mDescriptions = new string[kMSize];

	// Use this for initialization
	void Start () {
		loadDressDescriptions();
		LoadElmements ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	// function that changes mode, from man to woman and viceversa
	void ChangeMode () {
		Mode = !Mode;
		if (Mode)
			Character.sprite = Woman;
		else
			Character.sprite = Man;
		
		LoadElmements ();
	}

	/*void LoadClothes () {
		int distance = 0;
		if (Mode)
			for (int i = 0; i < fClothes.Length; i++) {
				Button element = Instantiate(element);
		        element.Image = fClothes[i];
			}
	}*/

	void LoadElmements() {

		Dress.sprite = null;
		text.text = null;

		// destroy container children
		foreach (Transform child in container.transform) {
			GameObject.Destroy(child.gameObject);
		}

		// check if we have a male or female
		if (Mode)
				itemCount = fClothes.Length;
		else
				itemCount = mClothes.Length;

		// obtain the rectransform component for our element and our container
		RectTransform rowRectTransform = itemPrefab.GetComponent<RectTransform> ();
		RectTransform containerRectTransform = container.GetComponent<RectTransform> ();

		//calculate the width and height of each child item
		float width = rowRectTransform.rect.width;
		float height = rowRectTransform.rect.height;
		int rowCount = itemCount / columnCount;

		if (itemCount % rowCount > 0)
				rowCount++;

		//adjust the width of the container so that it will just barely fit all its children
		float scrollWidth = width + (85 * itemCount);

		// container anchors
		containerRectTransform.offsetMin = new Vector2 (-scrollWidth / 2, containerRectTransform.offsetMin.y);
		containerRectTransform.offsetMax = new Vector2 (scrollWidth / 2, containerRectTransform.offsetMax.y);

		int j = 1;

		for (int i = 0; i < itemCount; i++) {

			//this is used instead of a double for loop because itemCount may not fit perfectly into the rows/columns
			//if (i % columnCount == 0)
				//j++;

			//create a new item, name it, and set the parent
			GameObject newItem = Instantiate (itemPrefab) as GameObject;
			newItem.name = itemPrefab.name + " item at " + i;
			//newItem.transform.parent = container.transform;
			newItem.transform.SetParent(container.transform,false);

			//move and size the new item
			RectTransform rectTransform = newItem.GetComponent<RectTransform> ();
//			Transform transform = newItem.GetComponent<Transform> ();

			float x = -containerRectTransform.rect.width / 2 + width * (i % itemCount);
			float y = containerRectTransform.rect.height / 2 - height * j;
			rectTransform.offsetMin = new Vector2(x, y);
			
			x = rectTransform.offsetMin.x + width;
			y = rectTransform.offsetMin.y + height;

			rectTransform.offsetMax = new Vector2(x, y);

			if(Mode)
				newItem.GetComponent<Image>().sprite = fClothes[i];
			
			else
				newItem.GetComponent<Image>().sprite = mClothes[i];

			newItem.GetComponent<Image>().type = Image.Type.Filled;

			newItem.GetComponent<index>().indexn = i;

			// function for the dress buttons
			newItem.GetComponent<Button>().onClick.AddListener(delegate {
					DressUp(newItem.GetComponent<index>().indexn);
			});
		}	
		
		// set the scrollrect value to 0
		scrollDress.value = 0;

		// generate a target dress
		GenerateTarget();
	}

	void DressUp(int index) {
		if (Mode) {
			Dress.sprite = fClothes [index];
			//text.text = fDescriptions [index]; this is for descriptive mode

			if (target == index) {
				message.text = "¡Muy bien! Siguiente vestido.";
				GenerateTarget();
			}

			else {
				message.text = "¡Vaya, has fallado!";
			}		

		} else {
			Dress.sprite = mClothes [index];
			//text.text = mDescriptions [index];
						
			if (target == index) {
				message.text = "¡Muy bien! Siguiente traje.";
				GenerateTarget();
			}
			
			else {
				message.text = "¡Vaya, has fallado!";
			}
		}

		displayMessage.SetTrigger("Trigger");
	}

	void GenerateTarget() {
		if (Mode) {
			target = Random.Range(0, kFSize);
			text.text = fDescriptions[target];
		}
		else {
			target = Random.Range(0, kMSize);
			text.text = mDescriptions[target];
		}

		scrollText.value = 1;
	}
	
	void loadDressDescriptions() {
		fDescriptions[0] = "<b>Mujer de clase baja de Edad Media (Ss.V-XV)</b>\r\nNuestra mujer lleva una <i>saya</i> de color azul con cuello redondeado acabado en pico. La  <i>saya</i> de  las mujeres eran llamadas también <i>tunica telar</i> ya que llegaban hasta los talones.\r\nLa <i>saya</i> de nuestra mujer tiene las mangas largas y estrechas y debajo de la túnica llevaba otra camisa larga que servía como ropa interior (esto se puede ver en el hombro del vestido). La <i>saya</i> se llevaba ajustada con un cinturón de cuero.";
		fDescriptions[1] = "<b>Mujer de clase alta de Edad Media (Ss.V-XV)</b>\r\nNuestra mujer de clase alta de la Edad Media lleva una <i>saya</i> o <i>túnica telar</i> decorada con estampados y con mangas largas. El cuello es en 'V' y está abierto por debajo del pecho. \r\nDebajo de ésta lleva otra camisa para tapar el escote. Y la lleva ceñida al cuerpo por un cinturón de tela o cuero.\r\n Además, estos vestidos podían llevar una <i>cola</i> que se llamaba <i>faldas</i> y que podía recogerse en la mano.";
		fDescriptions[2] = "<b>Mujer campesina de la Ilustración (s. XVIII)</b>\r\nLa mujer campesina de la ilustración está vestida con varias piezas. En la parte superior lleva un <i>corpiño</i> de tirante fino, que llega hasta la mitad del pecho,  de colores vivos, o <i>jubón</i>. Bajo este <i>corpiño</i> lleva una camisa de mangas largas, normalmente recogidas a mitad del brazo y un pañuelo que rellena el escote.\r\nLa parte inferior consiste en una falda amplia de telas estampadas que deja al descubierto los tobillos y un delantal largo y estrecho. \r\nBajo la falda lleva unas medias normalmente hechas de algodón y unos zapatos sencillos.";
		fDescriptions[3] = "<b>Mujer de clase alta de la Ilustración (S.XVIII)</b>\r\nNuestra mujer ilustrada lleva un conjunto de dos piezas. Podemos ver que viste una camisa blanca de de cuello alto con un pañuelo alrededor del cuello. También lleva una chaqueta (<i>Jacket</i>) de color azul que le cae por la parte trasera y que está recogida por la parte de delante. Como ropa interior lleva un <i>corsé</i>.\r\nEn la parte inferior lleva una <i>falda</i>. Debajo de ésta un <i>Polisón</i> que se colocaba en el trasero para que la <i>falda</i> abultara (se hacían de tela para dar calor o de aluminio para que pesara menos). La <i>falda</i> que lleva nuestra mujer es sencilla y lisa de color verde claro que solo se ha dibujado un trozo para dejar ver las <i>faldas</i> que servían de ropa interior.";
		fDescriptions[4] = "<b>Mujer clase baja del Renacimiento (Ss.XV-XVII)</b>\r\nLa vestimenta era más sencilla comparada con la de la nobleza.\r\nNuestra mujer lleva una <i>saya</i>, un vestido largo hasta los tobillos con mangas largas, de color verde. Sobre la <i>saya</i> lleva  una especie chaleco llamado <i>corpezuelo o jubón sin mangas</i>. Debajo de todo llevaban una casa larga que hacía de ropa interior que muchas veces sobre salía tapando el escote de la <i>saya</i>, en nuestra mujer lo vemos en el cuello blanco que asoma.\r\nLa <i>saya</i> solía ser de un material barato que podían permitirse estas mujeres.";
		fDescriptions[5] = "<b>La mujer noble en el Renacimiento (Ss. XV-XVII)</b>\r\nEl conjunto que lleva se llama 'modelo de corte'.\r\nLa parte superior es una <i>basquina</i> (corpiño) de manga larga que van abiertas dejando ver las mangas largas de la <i>saya</i> que lleva debajo, la <i>basquina</i> tiene una decoración de tela blanca fruncida (como si estuviera arrugada) \r\nLleva una falda muy larga que le tapa los pies.\r\nEn el cuello lleva una <i>grogera de abanillos de gasa rizada</i> (es el cuello blanco grande) y aunque nuestra mujer vaya de negro, los trajes solían tener decoración con hilos de colores.";
		fDescriptions[6] = "<b>Mujer de clase baja del s.XX</b>\r\nNuestra mujer lleva una blusa clara abotonada y decorada con dos lacitos. Las mangas son largas y son llamativas porque empiezan siendo abombadas (hinchadas) hasta el codo y se vuelven estrechas hasta la muñeca.\r\nLa falda es sencilla, de un color marrón, larga hasta los pies y con una forma acampanada.\r\nLas mujeres de la época solían vestir ropas ajustadas en la parte superior (camisas, corpiños...) y anchas en la inferior (faldas).";
		fDescriptions[7] = "<b>Mujer de clase alta del s.XX</b>\r\n Nuestra mujer lleva un conjunto de tres piezas.\r\nLa primera de ellas es un <i>corpiño</i> que va debajo de la ropa (en la imagen  se ha dibujado con líneas) y sirve para destacar la cintura de la mujer.\r\nLa segunda es una camisa sencilla de color marrón con botones y la tercera una falda larga en color claro; la falda está decorada con dos pliegues (arrugas) uno en horizontal y otro en vertical, haciendo que parezca que la falta está hecha en dos partes.\r\nPor último lleva un pañuelo atado al cuello.";
		fDescriptions[8] = "<b>Mujer hippie (½  s. XX)</b>\r\nNuestra mujer <i>hippie</i> lleva una falda amplia, hasta los tobillos, hecha de tela estampada con motivos florales. Ésta va ajustada al cuerpo mediante un cinturón fino para que no se resbale.\r\nLa parte superior consiste en un totade dos piezas: una camiseta o <i>blusa</i> amplia, cuyas mangas llegaban hasta la mitad de los brazos,  y un chaleco sin abrochar que caía hacia los lados del cuerpo.";
		fDescriptions[9] = "<b>La minifalda (s. XX-actualidad)</b>\r\nNuestra mujer contemporánea lleva una camiseta sencilla sin mangas. Las telas que se usaban podían ir estampadas para hacerlas más llamativas y hacerlas <i>a la moda</i>.\r\nLa minifalda es una falda que queda  muy por encima de las rodillas, dejando a la vista gran parte de las piernas, y que va sujeta a la cintura con un cinturón ancho, estilizando la figura de la mujer. \r\nEl conjunto va complementado con unas botas sencillas de caña alta y plataforma.";		
		
		mDescriptions[0] = "<b>Hombre clase baja medievo (Ss.V-XV)</b>\r\nNuestro hombre lleva una <i>saya</i> por las rodillas y abierta en el cuello y ajustada por un cordón.\r\nSus mangas son largas y está ajustada a la cintura con un cinturón. El cuello y los hombros los lleva cubiertos por un manto claro.\r\nLas piernas las lleva cubiertas por unas calzas claras que iban desde la cintura a los pies; podían usarse para caminar con ellas ya que se añadía cartón gordo o madera la planta de la calza para proteger el pie.\r\nAunque viste con ropas en tonos marrones, podían ser de diversos colores.";
		mDescriptions[1] = "<b>Hombre clase alta de Edad Media (Ss. V-XV)</b>\r\nLleva dos prendas una encima de la otra, una roja que se llama <i>saya</i> (como una camisa larga) y otra azul llamada <i>sobre-gonel</i>.\r\nLa <i>saya</i> es de manga larga y la lleva debajo del <i>sobre gonel</i>, es de mangas largas que podian ser <i>mangas haldas</i> (mangas abiertas) que dejan ver las mangas de abajo, <i>sobre-gonel</i> está abierto desde la cintura hasta el final por los laterales y tenía el cuello abierto y ajustado por un cordón.\r\nLas piernas las lleva cubiertas por unas <i>calzas</i> rojas que van desde la cintura a los pies, los cuales lleva calzados con zapatos oscuros.\r\nAunque los colores sean el azul y el rojo, las telas podían ser de cualquier color e incluso ir estampadas.";
		mDescriptions[2] = "<b>Hombre de clase baja de la Ilustración (S.XVIII)</b>\r\nNuestro hombre viste una camisa suelta, con escote en pico, y, sobre esta, una chaqueta corta. En la cintura lleva un <i>fajín</i> (una tela ceñida a los riñones para no hacerse daño trabajando).\r\nTambién lleva unos pantalones amplios y cortos llamados <i>calzones</i>. Se calza con unas medias <i>calzas</i> hasta la rodilla y los zapatos se ataban con cordones por encima del tobillo.\r\nLos colores no eran llamativos y las telas no eran de tan buena calidad como la de los de clase alta.";
		mDescriptions[3] = "<b>Hombre de clase alta de la Ilustración (S.XVIII)</b>\r\nNuestro hombre lleva un conjunto de inspiración francesa que llegó a España con la Ilustración.\r\n Viste una <i>camisola</i> simple de color beige y sobre esta una chaqueta abierta que se llaman <i>casaca</i> que tenía las mangas largas y estrechas y un pequeño cuello levantado que era el <i>cuello de tirilla</i>.\r\nLos pantalones que lleva se llaman <i>calzones</i> e iban pegados a las piernas, además nuestro hombre lleva unas botas altas que recuerdan a la de los militares.\r\nEstos trajes solían ser de buenas telas de colores e iban decorados con bordados en los puños de la <i>casaca</i> y en el <i>cuello de tirilla</i>.";
		mDescriptions[4] = "<b>Hombre de clase baja del Renacimiento (Ss.XV-XVII)</b>\r\nNuestro hombre va vestido con una camisa sencilla y sobre ésta lleva un <i>capote de dos baldas</i>, es una camisa más gruesa cerrada por delante y con las mangas abiertas de arriba abajo y cerradas en los puños con una tira pequeña, estas mangas se podían quitar del capote.\r\nTambién lleva unos pantalones llamados <i>calzas</i> que le llegan a la rodilla y unos zapatos que solían usar la gente trabajadora que se llaman <i>botas con polainas</i>, eran unas botas con un recubiertas de piel para protegerlas.";
		mDescriptions[5] = "<b>Hombre noble del Renacimiento (Ss.XV-XVII)</b>\r\nLos hombres nobles vestían el mismo tipo de traje, solo cambiaba la calidad de la tela y en la corte iban de negro.\r\nNuestro hombre está vestido con tonos negros y lleva un chaleco (<i>coleto</i>) que está relleno de algodón para que el pecho pareciera más grande, este <i>coleto</i> no se podía lavar. El cuello blanco se podía quitar y se llamaba <i>golilla</i>, se llevaba atado al cuello del chaleco y servían para protegerlo del sudor y del roce. Nuestro noble no lleva camisa, las mangas eran de quita y pon y estaban, también, atadas al chaleco para poder lavarlas con comodidad.\r\nLos pantalones cortos que lleva se llamaban <i>calzas de calabaza</i>. Se podían rellenar con serrín, paja o papel para que fueran más pomposas, se llevaban atadas al chaleco con unas cintas (los lazos que se ven en el dibujo). Debajo de las <i>calzas</i> lleva unas medias que podían ser de tela o de punto.";
		mDescriptions[6] = "<b>Hombre clase baja s.XX</b>\r\nNuestro hombre lleva puesta una camisa beige con botones que lleva por dentro de los pantalones. Las mangas son largas, pero las lleva remangadas hasta los codos.\r\nLleva pantalones sencillos en color marrón claro ajustados por un cinturón de cuero marrón.\r\nLos zapatos son básicos en un color claro, solían ser zapatos para el trabajo.";
		mDescriptions[7] = "<b>Hombre clase alta del s.XX</b>\r\nNuestro hombre viste un <i>frac</i>. El <i>frac</i> tiene este nombre por la chaqueta.\r\nLa chaqueta es negra, de manga hasta las muñecas y con la solapa vuelta y de pico y va abotonada, pero lo más llamativo son las dos solapas que caen por detrás.\r\nDebajo de la chaqueta lleva un chaleco negro, una camisa de cuello alto y una corbata negra también. \r\nLos pantalones son grises y lleva unos zapatos negros.\r\nA este traje se le suele llamar <i>traje de pingüino</i> ya que recuerda a la forma y colores de los pingüinos.";
		mDescriptions[8] = "<b>Hombre hippie (½ s. XX)</b>\r\nLo más destacable de la vestimenta de nuestro hombre <i>hippie</i> son los pantalones de campana; ajustados hasta la altura de la rodilla,desde donde empiezan a hacerse más anchos, creando una forma de <i>campana</i> (de ahí su nombre) y ajustados por un cinturón a la altura de la cintura.\r\nLa parte superior del conjunto es una camisa de manga larga, con algunos botones desabrochados, y que queda dentro del pantalón./r/ Lo normal era que fueran de colores vivos o con  estampados en las telas.";
		mDescriptions[9] = "<b>Los vaqueros (ss. XX-Actualidad)</b> \r\nNuestro hombre va vestido con unos pantalones ajustados, de tela gruesa y azulada, llamados <i>vaqueros</i>.\r\nAunque en la parte superior se pueden llevar tanto camisas como camisetas, el modelo lleva una camiseta básica de color plano y mangas cortas.\r\n";
	}

	public void CloseGame() {
		Application.Quit();
	}
}
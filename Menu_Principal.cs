using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Principal : MonoBehaviour
{
    // Estados del menú
    private enum MenuState { Main, Instructions, Credits, Settings }
    private MenuState currentState = MenuState.Main;

    // Imágenes para las pantallas de instrucciones y créditos (asignar desde el Inspector)
    public Texture2D instructionImage;
    public Texture2D creditsImage;

    // Variables de ajustes
    private float quality = 3f;         // Rango ejemplar: 0 a 5
    private float sensibilidad = 1f;    // Rango ejemplar: 0.1 a 10
    private float volume = 1f;          // Rango: 0 a 1

    private void OnGUI()
    {
        // Selecciona qué pantalla mostrar según el estado actual
        switch (currentState)
        {
            case MenuState.Main:
                MostrarMenuPrincipal();
                break;
            case MenuState.Instructions:
                MostrarInstrucciones();
                break;
            case MenuState.Credits:
                MostrarCreditos();
                break;
            case MenuState.Settings:
                MostrarAjustes();
                break;
        }
    }

    // MENÚ PRINCIPAL
    private void MostrarMenuPrincipal()
    {
        int anchoBoton = 200;
        int altoBoton = 50;
        int espacio = 10;
        int posX = Screen.width / 2 - anchoBoton / 2;
        int posY = 100;

        if (GUI.Button(new Rect(posX, posY, anchoBoton, altoBoton), "Empezar"))
        {
            // Cambia al escenario 0
            SceneManager.LoadScene(0);
        }
        posY += altoBoton + espacio;
        if (GUI.Button(new Rect(posX, posY, anchoBoton, altoBoton), "Instrucciones"))
        {
            currentState = MenuState.Instructions;
        }
        posY += altoBoton + espacio;
        if (GUI.Button(new Rect(posX, posY, anchoBoton, altoBoton), "Créditos"))
        {
            currentState = MenuState.Credits;
        }
        posY += altoBoton + espacio;
        if (GUI.Button(new Rect(posX, posY, anchoBoton, altoBoton), "Ajustes"))
        {
            currentState = MenuState.Settings;
        }
        posY += altoBoton + espacio;
        if (GUI.Button(new Rect(posX, posY, anchoBoton, altoBoton), "Salir"))
        {
            Application.Quit();
        }
    }

    // PANTALLA DE INSTRUCCIONES
    private void MostrarInstrucciones()
    {
        int margen = 20;
        GUI.Label(new Rect(margen, margen, Screen.width - 2 * margen, 30), "Reglas del juego:");
        GUI.Label(new Rect(margen, margen + 30, Screen.width - 2 * margen, 60), 
            "Aquí se describen las reglas de cómo jugar. " +
            "Por ejemplo: Mueve al personaje, recoge objetos y evita obstáculos.");

        // Muestra una imagen con instrucciones, si se asignó
        if (instructionImage != null)
        {
            int anchoImg = 300;
            int altoImg = 200;
            GUI.DrawTexture(new Rect(Screen.width / 2 - anchoImg / 2, margen + 100, anchoImg, altoImg), instructionImage, ScaleMode.ScaleToFit);
        }

        // Botón para regresar al menú principal
        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height - 80, 200, 50), "Regresar"))
        {
            currentState = MenuState.Main;
        }
    }

    // PANTALLA DE CRÉDITOS
    private void MostrarCreditos()
    {
        int margen = 20;
        GUI.Label(new Rect(margen, margen, Screen.width - 2 * margen, 30), "Créditos del Museo:");
        GUI.Label(new Rect(margen, margen + 30, Screen.width - 2 * margen, 60),
            "Aquí se muestran los créditos del museo, incluyendo nombres de diseñadores, desarrolladores y colaboradores.");

        // Muestra una imagen de créditos, si se asignó
        if (creditsImage != null)
        {
            int anchoImg = 300;
            int altoImg = 200;
            GUI.DrawTexture(new Rect(Screen.width / 2 - anchoImg / 2, margen + 100, anchoImg, altoImg), creditsImage, ScaleMode.ScaleToFit);
        }

        // Botón para regresar al menú principal
        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height - 80, 200, 50), "Regresar"))
        {
            currentState = MenuState.Main;
        }
    }

    // PANTALLA DE AJUSTES
    private void MostrarAjustes()
    {
        int margen = 20;
        GUI.Label(new Rect(margen, margen, Screen.width - 2 * margen, 30), "Ajustes de la aplicación");

        // Ajuste de calidad
        GUI.Label(new Rect(margen, margen + 40, 150, 30), "Calidad: " + quality.ToString("F1"));
        quality = GUI.HorizontalSlider(new Rect(margen + 160, margen + 45, 200, 30), quality, 0f, 5f);

        // Ajuste de sensibilidad (mouse/arrastre)
        GUI.Label(new Rect(margen, margen + 80, 150, 30), "Sensibilidad: " + sensibilidad.ToString("F1"));
        sensibilidad = GUI.HorizontalSlider(new Rect(margen + 160, margen + 85, 200, 30), sensibilidad, 0.1f, 10f);

        // Ajuste del volumen de audio general
        GUI.Label(new Rect(margen, margen + 120, 150, 30), "Volumen: " + volume.ToString("F1"));
        volume = GUI.HorizontalSlider(new Rect(margen + 160, margen + 125, 200, 30), volume, 0f, 1f);
        AudioListener.volume = volume;  // Aplica el volumen global

        // Botón para regresar al menú principal
        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height - 80, 200, 50), "Regresar"))
        {
            currentState = MenuState.Main;
        }
    }
}

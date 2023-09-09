using Google.Cloud.Translation.V2;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using static System.Net.Mime.MediaTypeNames;
using Translate.API.Models;
using System.Reflection;
using Amazon;
using Amazon.Translate;
using Amazon.Translate.Model;

using Microsoft.Extensions.Configuration;
using System.Globalization;

[ApiController]
[Route("api/[controller]")]
public class TranslationController : ControllerBase
{
    private readonly TranslationClient _translationClient;



    public TranslationController()
    {
        // Replace "YOUR_API_KEY" with your actual API key
        _translationClient = TranslationClient.CreateFromApiKey("AIzaSyBvTk7vKEx3wmRlQlocMS0qdfVbnnq_FBw");

    }


    [HttpGet("translate")]

    public async Task<IActionResult> TranslateText(string text, string code)
    {


        TranslationResult translationResult = await _translationClient.TranslateTextAsync(
            text,
            targetLanguage: code);

        string translatedText = translationResult.TranslatedText;

        return Ok(translatedText);
    }

    [HttpPost("translateArray")]
    public async Task<IActionResult> TranslateTextArray(string[] words, string code)
    {

        string[] newWords = new string[words.Length];

        for (int i = 0; i < words.Length; i++)
        {
            TranslationResult translationResult = await _translationClient.TranslateTextAsync(
            words[i],
            targetLanguage: code);

            string translatedText = translationResult.TranslatedText;

            newWords[i] = translatedText;
        }

        return Ok(newWords);
    }

    [HttpPost("TranslateObject")]
    public async Task<IActionResult> TranslateObject([FromBody] TranslateModel translateModel, string code)
    {
        TranslateModel newTranslateModel = new TranslateModel();
        foreach (PropertyInfo prop in translateModel.GetType().GetProperties())
        {


            TranslationResult translationResult = await _translationClient.TranslateTextAsync(
            prop.GetValue(translateModel).ToString(),
            targetLanguage: code);

            string translatedText = translationResult.TranslatedText;

            prop.SetValue(newTranslateModel, translatedText);
        }

        return Ok(newTranslateModel);
    }

    // word by word translate
    // Having problem in string = "BreakIn"
    [HttpPost]
    [Route("translateCustomization/{code}")]
    
    public async Task<IActionResult> translateArray([FromRoute] string code, [FromBody] string[] textArray)
    {
        string[] resultantArray = new string[textArray.Length];
  

        // If code = "en" then there is no need for translation
        if(code != "en")
        {

            // textArray = ["QC", "NCB", "SCPA", "CLL", "MISC"] for tamil
            // textArray = ["GCV", "PCV", "MISC", "CLL", "SCPA", "IAS" ] for telugu
            // textArray = ["QC", "GCV", "GCV TP", "PCV", "PCV TP", "MISC S TP", "CLL", "RTO"] for marathi
            for (int i = 0; i < textArray.Length; i++)
            {
                string text = textArray[i];
                string resultantText = "";
                foreach (string str in text.Split(' '))
                {

                    // Customization for string = "IAS" and code != "hi"
                    // for code = "hi" it is converting the string = "IAS" to hindi so need to iterate it char by char

                    if(str == "IAS" && code != "hi")
                    {
                        string newStr = "";
                        foreach(char c in str)
                        {
                            TranslationResult translationResult = await _translationClient.TranslateTextAsync(
                            c.ToString(),
                            targetLanguage: code);

                            string translatedText = translationResult.TranslatedText;
                            newStr += translatedText;
                        }

                        resultantText += newStr + " ";
                     
                    }

                    // Customization for the string = "IAS" and code = "hi" 

                    if(str == "IAS" && code == "hi")
                    {
                        TranslationResult translationResult = await _translationClient.TranslateTextAsync(
                            str,
                            targetLanguage: code);

                        string translatedText = translationResult.TranslatedText;
                        resultantText += translatedText + " ";

                    }

                    // Customization for string = "NYSA" and code = <any>
                    // If string = "NYSA" make it "NAYSA" for the translator to understand

                    if(str == "NYSA")
                    {
                        string newStr = "NAYSA";
                        TranslationResult translationResult = await _translationClient.TranslateTextAsync(
                            newStr,
                            targetLanguage: code);

                        string translatedText = translationResult.TranslatedText;

                        // adding the translated text to resulatant text 
                        resultantText += translatedText + " ";

                    }

                    

                    if(str != "NYSA" && str != "NAYSA" && str != "IAS") 
                    {
                        TranslationResult translationResult = await _translationClient.TranslateTextAsync(
                       str,
                       targetLanguage: code);

                        string translatedText = translationResult.TranslatedText;

                        resultantText = resultantText + translatedText + " ";
                    }

                 
                }

                resultantArray[i] = resultantText;

            }
        }
        else
        {
            resultantArray = textArray;
        }
       
        return Ok(resultantArray);
    }

    [HttpPost]
    [Route("translationNewCustomization/{code}")]
    public async Task<IActionResult> translateNewCustomization([FromRoute] string code, [FromBody] string[] textArray)
    {

        string[] resultantArray = new string[textArray.Length];


        // If code = "en" then there is no need for translation
        if (code != "en")
        {
            for (int i = 0; i < textArray.Length; i++)
            {

                string text = textArray[i];

                // If the text contains string = "NYSA" or "IAS" or "Nysa"
                //    then break the string into words and translate word by word.
                if (text.Contains("NYSA") || text.Contains("IAS") || text.Contains("Nysa"))
                {

                    string resultantString = "";
                    foreach (string str in text.Split(' '))
                    {

                        // If string = "Nysa" then translate "Naysa" and not "Nysa" 
                        if (str == "Nysa")
                        {
                            string newStr = "Naysa";
                            TranslationResult translationResult = await _translationClient.TranslateTextAsync(
                            newStr,
                            targetLanguage: code);

                            string translatedText = translationResult.TranslatedText;

                            resultantString += translatedText + " ";
                        }
                        else
                        {
                            TranslationResult translationResult = await _translationClient.TranslateTextAsync(
                            str,
                            targetLanguage: code);

                            string translatedText = translationResult.TranslatedText;

                            resultantString += translatedText + " ";
                        }




                    }

                    resultantArray[i] = resultantString;
                }
                else
                {
                    TranslationResult translationResult = await _translationClient.TranslateTextAsync(
                           text,
                           targetLanguage: code);

                    string translatedText = translationResult.TranslatedText;

                    resultantArray[i] = translatedText;
                }
            }
        }
        else if(code == "en")
        {
            resultantArray = textArray;
            
        }

        

        return Ok(resultantArray);
    }




}

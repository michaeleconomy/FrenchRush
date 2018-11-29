using System;
using UnityEngine;
using UnityEngine.UI;

public class UIDialog : MonoBehaviour {
    static UIDialog instance;

    public delegate void Callback(int buttonClicked, string input);
    public delegate void ConfirmCallback();
    public delegate void AlertCallback();
    public delegate void PromptCallback(string response);


    public GameObject dialog;
    public Text messageText;
    public InputField textField;
    public Transform buttonList;
    public Button buttonPrefab;

    private void Awake() {
        instance = this;
    }

    public static void PromptForString(string message, string defaultValue, PromptCallback callback) {
        Display(message, new string[] { "OK", "Cancel" }, true, defaultValue, (buttonClicked, input) => {
            if (buttonClicked == 0) {
                callback(input);
                return;
            }
            callback(null);
        });
    }


    public static void PromptForString(string message, PromptCallback callback) {
        PromptForString(message, null, callback);
    }

    public static void Confirm(string message, ConfirmCallback callback) {
        Display(message, new string[] { "Confirm", "Cancel" }, false, null, (buttonClicked, input) => {
            if (buttonClicked == 0) {
                callback();
            }
        });
    }

    public static void Alert(string message, AlertCallback callback = null) {
        Display(message, new string[] { "OK" }, false, null, (buttonClicked, input) => {
            if(callback != null) {
                callback();
            }
        });
    }



    public static void Display(string message,
                               string[] buttonLabels,
                               bool showField,
                               string defaultValue,
                               Callback callback) {
        instance.InternalDisplay(message,
                                 buttonLabels,
                                 showField,
                                 defaultValue,
                                 callback);
    }


        private void InternalDisplay(string message,
                                 string[] buttonLabels,
                                 bool showField,
                                 string defaultValue,
                                 Callback callback) {
        if (buttonLabels == null || buttonLabels.Length == 0) {
            throw new Exception("button labels are required");
        }
        textField.gameObject.SetActive(showField);
        textField.text = defaultValue;
        messageText.text = message;
        buttonList.DeleteChildren();
        for (var i = 0; i < buttonLabels.Length; i++) {
            var buttonLabel = buttonLabels[i];
            var button = Instantiate(buttonPrefab, buttonList, false);
            var text = button.GetComponentInChildren<Text>();
            text.text = buttonLabel;
            var buttonIndex = i;
            button.onClick.AddListener(() => {
                dialog.SetActive(false);
                if (callback != null) {
                    callback(buttonIndex, textField.text);
                }
            });
        }
        dialog.SetActive(true);
    }
}

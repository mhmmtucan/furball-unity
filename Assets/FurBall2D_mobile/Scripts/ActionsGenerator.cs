using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActionsGenerator : MonoBehaviour
{
    public GameObject if_prefab;
    public GameObject repeat_prefab;
    public GameObject functionsPrefab;
	public GameObject conditionsPrefab;
	public GameObject furry;
    public GameObject endPanel;
    PlayerController_m pc;

    public void addActionsToView (string actionName)
	{
		var panel = GameObject.Find ("ActionList");

		if (actionName.Equals ("if")) {
			GameObject button = (GameObject)Instantiate (if_prefab);
			button.GetComponent<RectTransform> ().SetParent (panel.transform);

		} else if (actionName.Equals ("for")) {
			GameObject button = (GameObject)Instantiate (repeat_prefab);
            button.GetComponent<RectTransform> ().SetParent (panel.transform);

		} else if (actionName.Equals ("functions")) {
			GameObject functions = (GameObject)Instantiate (functionsPrefab);
			functions.GetComponent<RectTransform> ().SetParent (panel.transform);

		} else if (actionName.Equals ("conditions")) {
			GameObject conditions = (GameObject)Instantiate (conditionsPrefab);
			conditions.GetComponent<RectTransform> ().SetParent (panel.transform);
		}
	}

    public void executeCommands ()
	{
        pc.transform.position = SceneInformation.instance.starting_position;
        StartCoroutine(Execute());
	}

    IEnumerator Execute() {
        var panel = GameObject.Find("ActionList");
        int childCount = panel.transform.childCount;
        //string[] commands = new string[childCount];
        int repeat_index = -1;
        int repeat_number = -1;
        int collected_objects = 0;

        for(int i = 0; i < childCount; i++) {
            //ith child is the ith command
            //child 0 is the text (command)
            //child 1 will be name of the variable for if, execution number for repeat
            //commands[i] = panel.transform.GetChild(i).GetChild(0).GetComponent<Text>().text;
            //Debug.Log(commands[i]);

            Transform child = panel.transform.GetChild(i);
            string command = child.GetChild(0).GetComponent<Text>().text;
            Debug.Log("command is: " + command);

            if(command == "move()") {
                pc.hor = 1;
                pc.target_position = pc.transform.position + 8 * Vector3.one;
                yield return new WaitForSeconds(2.0f);
            }
            if(command == "collect()") {
                Debug.Log("collecting");
                if(SceneInformation.instance.variables["white cloud"]) {
                    //we actually have the collectable near us
                    SceneInformation.instance.collectables.Remove(SceneInformation.instance.variables["white cloud"]);
                    Destroy(SceneInformation.instance.variables["white cloud"]);
                    SceneInformation.instance.variables["white cloud"] = null;
                    collected_objects++;
                }
                else {
                    //we did not have the collectable
                    //we are gonna just break the loop and execution
                    //this is a failure
                    break;
                }
                yield return null;
            }
            else if(command == "jump()") {//instead of just jumping do a function that does jump_and_move same for double jump
                pc.jumping = true;
                yield return null;
            }
            else if(command == "double_jump()") {
                pc.jumping = true;
                yield return null;
                pc.jumping = true;
                yield return null;
            }
            else if(command == "if") {
                //get the variable name check it, if it is true do the next command. dont forget to increment the i
                //if the condition is true we need to do the command after if variable
                //if the condition is false we need to skip the command after if variable
                string variable = child.GetChild(1).GetChild(0).GetComponent<Text>().text;
                GameObject value = null;
                if(SceneInformation.instance.variables.TryGetValue(variable, out value)) {
                    //we get the value
                    if(value) {
                        //the condition is true we need to execute the next command
                        //we have the game object
                    }
                    else {
                        //the condition is false we wont execute the next command
                        //we don not have the game object
                        i++;
                    }
                }
                else {
                    //name does not exists in the dictionary
                }
                yield return null;
            }
            else if(command == "repeat") {
                //get the next command to know how many times to execute
                //get the next command and execute it, when it finishes go back to repeat command again
                string number = child.GetChild(1).GetChild(0).GetComponent<Text>().text;
                Debug.Log(number);

                repeat_index = i;
                int.TryParse(number, out repeat_number);
                yield return null;
            }
            else {
                //idle
            }

            if(GameInformation.dead) break;

            if(i >= childCount - 1 && repeat_index != -1) {
                i = repeat_index;
                repeat_number--;

                if(repeat_number <= 0) {
                    break;
                }
            }
        }

        if(Mathf.Abs(SceneInformation.instance.finish_line.transform.position.x - pc.transform.position.x) < 2f && SceneInformation.instance.collectables.Count == 0) {
            endPanel.gameObject.SetActive(true);
            endPanel.GetComponentInChildren<Text>().text = "Well Done!";
            endPanel.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "Next Level";
        }
        else {
            endPanel.gameObject.SetActive(true);
            endPanel.GetComponentInChildren<Text>().text = "Failure";
            endPanel.GetComponentInChildren<Text>().enabled = true;
            endPanel.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "Try Again";
        }
    }

    public void regulateLevel() {
        if (endPanel.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text == "Next Level") {
            UnityEngine.SceneManagement.SceneManager.LoadScene(++(GameInformation.level));
        }
        else {
            UnityEngine.SceneManagement.SceneManager.LoadScene(GameInformation.level);
        }
    }

    void Awake() {
        pc = furry.GetComponent<PlayerController_m>();
    }
}

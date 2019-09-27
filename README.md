# Sports-Score-Tracker
3rd Year Mobile Applications cross platform Xamarin Application which keeps track of scores for multiple sports including Soccer, Basketball, GAA, Ice Hockey, Tennis and Rugby.

### Visual Studio Version:
Visual Studio 2017

### NuGet Packages used: 
- Newtonsoft.Json
- Xam.Plugin.SimpleAudioPlayer

## Functionality
- The main functionality of this application is to allow users to record scores of the 6 sports mentioned above using the rules/scoring system of each sport implemented as they are in real life.
- The user can then save these games to their device in order to view at a later date.
- Before saving users have an option if they want a sound to play upon saving their game by double tapping the sound icon in the top left.
- User is also given the ability to reset the score if they want while recording the score.
- The user must enter a match name which also cannot be a duplicate name, in order to save their game
- Each sport has its own rule sets implemented, e.g. tennis, score cannot go above 40 and if both players/teams are tied at 40 a piece, a deuce is entered for which a winner is picked. 
- The users can then access their saved games and update them by selecting them from the list of games, which can then be saved again by pressing the update button.
- Delete functionality also exists for the user so that they can remove selected games from the list of games.

## Architecture
- The method of storing the matches is done via a match class. A constructor is used to create a match class, populated by the information the user enters.
- This class also contains methods used on the class, for example reading and writing the data to and from files.
- This class is used throughout the application in all pages except for the mainpage for reusability.
- The mainpage is set up as a NavigationPage in order to navigate accross the app to other pages and return to it easily for a nice fluent smooth feel for the users navigation.
- The files are stored locally,so that if the user deletes the application and decides to download it later, they will not lose their previously saved data, this can be deleted by the user in their files on their system if they wish to do so. If application was on app store, would have to provide option to delete file when uninstalling.
- No passwords/usernames needed, so user's security data doesn't come under risk if application or device was compromised in any way.

## Implementation
- The MainPage is setup as a Navigation page as previously stated with images displayed as icons for user navigation, these images are accessed by tap gesture recognisers (single tap) to navigate to the desired page. I was going to use image buttons, but didnt look as asthetically pleasing. The mainpage also contains a button navigating to the load games page.
- Each build e.g. UWP,Android, have thier own backgrounds so that the user experience is unique on each each platform, this is due to screen size issues with larger images on smaller devices making the application look less appealling.
- Each sports page contains unique elements needed for that particular sport while maintaining a similar feel for the user throughout each sports page. Alerts are displayed for the user when they have not entered correct details on each page, research was done on this to look at how the different types of alerts are used e.g. regular DisplayAlert returning boolean values vs DisplayActionSheet returning a string value chosen by user - references are in code. 
- Each sports page also consists of a mute button which either prevents the sport from playing its unique sound when deactivated or plays it when activated, this is implemented using tap gesture recognisers (double tap) which changes the icon so that the user knows if sound is muted or not. The sound is played though the audio player implemented from the nuGet packages, which was found through researching online,again references are in code.
- Each page also has a reset button which is used to reset the scores for each page to 0. This is again implemented as an image with tap gesture recognisers (double tap).
- When the user enters an acceptable name(non duplicate) then the game is written to a file, game names are trimmed to prevent white space from causing confusion for the user as to why a game may not be deleting etc... Saving games is carried out by converting items in the saved games list to a JSON object and writing to file.
- The Load games page is used to display the list of saved games by reading from the text file and deserializing the JSON, which is then displayed in a listview. when an item is selected from the listview, it's information is populated into the entry elements on the page to allow the user to update the game information (achieved through data binding) and save or they can delete the selected item. Both are done by looping through the list and either updating to the elements text or removing them from the list. Then refreshing the listview for the user
- The INotifyPropertyChanged element is also used in order to show the user the changes they are making to the game in real time on the list view


Note: Sometimes upon first build on android, background image doesn't fully cover background, but works fine when navigating from and returning back to mainpage, also works fine when loading build when application already built to phone. (Assuming it's a bug with building, researched and couldn't find solution to issue) Note: Does not have IOS implementation.

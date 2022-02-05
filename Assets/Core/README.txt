CREATING A NEW GAME TEMPLATE
	create new repo in folder
	copy assets, [etc] into new folder from latest project
	add to unity hub and open
	Edit > Preferences > ensure that Visual Studio Community is as the default editor
	open package manager and update any out of date packages
	delete unwanted stuff from assets from within Unity

CORE DEV TASKS
AudioManager...
> soundtrack player
	> create a separate SoundtrackCue that will handle snoundtrack requests - these are always going to be different.
		> create a parent object (AudioCueParent) with two subtypes: AudioCue and SoundtrackCue
	> should be different to the general sfx channels - maybe it is a seperate standalone module that takes stuff in from somewhere else
	> when a cue is a soundtrack object then it should trigger this new treatment
	> soundtrack module contains:
		> direct link to timer
		> dedicated audio srcs
> AudioRequest can probably be deleted

There are 4 main types of audio: SFX, Soundtrack, UI, Ambience
	UI - should be played with no spatials - purely for UI feedback
	Soundtrack - no spatials - could be timed - can be changed by game
	> create a new test scene for more modular audiomanager
		> create a subsidiary UI that can be plugged into a key driven widget e.g. the UI is tweakable but basically fixed and is then a plugin component. You can plug it into a seperate UI component as a child so it pops up into being etc
			> the outcome is you can press V and a UI pops up that contains the modular audio levels/toggles
		? should AudioCue be exposed ?
		> play music in a mixer that keeps tempo of different parts together


Core Application Manager, Menu and Audio Manager

SANDBOX AND GAME SCENES
The Sandbox and Game scenes are just simple scenes.
Create new scenes outside of the Core folder so you can update the Core with fixes.
Remove the test Sandbox and Game scenes from the build settings

SETUP APPLICATION MANAGER
1. Add all scenes to Build Settings
2. Use EXACT string names for scenes in Application Manager/SceneLoader/Scenes
3. Attach the MainCamera.cs to all main cameras

SETUP GAME LAYERS
1. create layers in inspector to match layers in GameLayers script

SETUP AUDIO MANAGER


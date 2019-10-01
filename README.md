# rn-ink-canvas

!!! Only for Windows UWP (current) !!!

## Getting started

`$ npm install rn-ink-canvas --save`

### Mostly automatic installation

`$ react-native link rn-ink-canvas`

### Manual installation

#### iOS

1. In XCode, in the project navigator, right click `Libraries` ➜ `Add Files to [your project's name]`
2. Go to `node_modules` ➜ `rn-ink-canvas` and add `RNInkCanvas.xcodeproj`
3. In XCode, in the project navigator, select your project. Add `libRNInkCanvas.a` to your project's `Build Phases` ➜ `Link Binary With Libraries`
4. Run your project (`Cmd+R`)<

#### Android

1. Open up `android/app/src/main/java/[...]/MainApplication.java`
  - Add `import com.legion-zver.rn-ink-canvas.RNInkCanvasPackage;` to the imports at the top of the file
  - Add `new RNInkCanvasPackage()` to the list returned by the `getPackages()` method
2. Append the following lines to `android/settings.gradle`:
  	```
  	include ':rn-ink-canvas'
  	project(':rn-ink-canvas').projectDir = new File(rootProject.projectDir, 	'../node_modules/rn-ink-canvas/android')
  	```
3. Insert the following lines inside the dependencies block in `android/app/build.gradle`:
  	```
      compile project(':rn-ink-canvas')
  	```

#### Windows
[Read it! :D](https://github.com/ReactWindows/react-native)

1. In Visual Studio add the `RNInkCanvas.sln` in `node_modules/rn-ink-canvas/windows/RNInkCanvas.sln` folder to their solution, reference from their app.
2. Open up your `MainPage.cs` app
  - Add `using Ink.Canvas.RNInkCanvas;` to the usings at the top of the file
  - Add `new RNInkCanvasPackage()` to the `List<IReactPackage>` returned by the `Packages` method


## Usage
```javascript
import RNInkCanvas from 'rn-ink-canvas';

// TODO: What to do with the module?
RNInkCanvas;
```

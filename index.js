import React from 'react';
import * as PropTypes from 'prop-types';
import { UIManager, View, Platform, requireNativeComponent, findNodeHandle } from 'react-native';

// noinspection JSUnusedGlobalSymbols
class InkCanvas extends React.Component {

    static propTypes = {
        initStrokes: PropTypes.array,
        lineColor: PropTypes.string,
        lineWidth: PropTypes.number,
        onChange: PropTypes.func,
        onExport: PropTypes.func,
        readOnly: PropTypes.bool,
        style: PropTypes.any,
    };

    static defaultProps = {
        lineColor: '#000000',
        readOnly: false,
        lineWidth: 1,
    };

    _nativeRef = null;

    _onChangeNativeRef = (ref) => {
        this._nativeRef = ref;
    };

    onChange = (event) => {
        if (!this.props.onChange) {
            return;
        }
        // noinspection JSUnresolvedVariable
        this.props.onChange((event.nativeEvent || {}).strokes || []);
    };

    onExport = (event) => {
        if (!this.props.onExport) {
            return;
        }
        // noinspection JSUnresolvedVariable
        this.props.onExport((event.nativeEvent || {}).base64 || '');
    };

    runCommand = (name, args = []) => {
        if (this._nativeRef) {
            const handle = findNodeHandle(this._nativeRef);
            if (!handle) {
                throw new Error('Cannot find node handles');
            }
            Platform.select({
                default: () => {
                    // noinspection JSUnresolvedVariable
                    const commandId = UIManager.RNInkCanvas.Commands[name] || 0;
                    if (!commandId) {
                        throw new Error(`Cannot find command ${name} in RNInkCanvas manager!`);
                    }
                    UIManager.dispatchViewManagerCommand(handle, commandId, args);
                },
                ios: () => {
                    // noinspection JSUnresolvedVariable
                    NativeModules.RNInkCanvasManager[name](handle, ...args);
                },
            })();
        } else {
            throw new Error('No ref to RNInkCanvas component, check that component is mounted');
        }
    };

    clear = () => {
        this.runCommand('clear');
    };

    setStrokes = (strokes = []) => {
        this.runCommand('setStrokes', [strokes]);
    };

    // noinspection JSUnusedGlobalSymbols
    requestExportImageBytes = () => {
        this.runCommand('exportImageBytes');
    };

    componentDidMount() {
        if ((this.props.initStrokes || []).length > 0) {
            this.setStrokes(this.props.initStrokes);
        }
    }

    render() {
        const { style, initStrokes, ...props } = this.props || {};
        return (
            <View style={style}>
                <RNInkCanvas {...(props || {})}
                             ref={this._onChangeNativeRef}
                             style={{flex: 1, width: '100%', height: '100%'}}
                             onChange={this.onChange} onExport={this.onExport} />
            </View>
        );
    }
}

const RNInkCanvas = requireNativeComponent('RNInkCanvas', InkCanvas, {
    nativeOnly: {
        onChange: true,
        onExport: true
    }
});

export default InkCanvas;

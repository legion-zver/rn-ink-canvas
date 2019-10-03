import React from 'react';
import * as PropTypes from 'prop-types';
import { View, requireNativeComponent } from 'react-native';

// noinspection JSUnusedGlobalSymbols
class InkCanvas extends React.Component {

    static propTypes = {
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
        this.props.onChange((event.nativeEvent || {strokes: []}).strokes || []);
    };

    onExport = (event) => {
        if (!this.props.onExport) {
            return;
        }
        this.props.onExport((event.nativeEvent || {base64: ''}).base64 || '');
    };

    clear = () => {
        if (!this._nativeRef) {
            return;
        }
        'clear' in this._nativeRef &&
        this._nativeRef.clear();
    };

    setStrokes = (strokes = []) => {
        if (!this._nativeRef) {
            return;
        }
        'setStrokes' in this._nativeRef &&
        this._nativeRef.setStrokes(strokes);
    };

    // noinspection JSUnusedGlobalSymbols
    requestExportImageBytes = () => {
        if (!this._nativeRef) {
            return;
        }
        'exportImageBytes' in this._nativeRef &&
        this._nativeRef.exportImageBytes();
    };

    render() {
        const { style, ...props } = this.props || {};
        return (
            <View style={style}>
                <RNInkCanvas {...(props || {})}
                             onChange={this.onChange}
                             onExport={this.onExport}
                             ref={this._onChangeNativeRef}
                             style={{flex: 1, width: '100%', height: '100%'}} />
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

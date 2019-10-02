import React from 'react';
import * as PropTypes from 'prop-types';
import { requireNativeComponent } from 'react-native';

const RNInkCanvas = requireNativeComponent('RNInkCanvas', InkCanvas, {
    nativeOnly: {onChange: true}
});

export default class InkCanvas extends React.PureComponent {

    static propTypes = {
        lineColor: PropTypes.string,
        lineWidth: PropTypes.number,
        onChange: PropTypes.func,
        onExport: PropTypes.func,
        readOnly: PropTypes.bool,
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
        this.props.onChange(event.nativeEvent);
    };

    onExport = (event) => {
        if (!this.props.onExport) {
            return;
        }
        this.props.onExport(event.nativeEvent);
    };

    clear = () => {
        if (!this._nativeRef) {
            return;
        }
        'clear' in this._nativeRef && this._nativeRef.clear();
    };

    setStrokes = (strokes = []) => {
        if (!this._nativeRef) {
            return;
        }
        'setStrokes' in this._nativeRef && this._nativeRef.setStrokes(strokes);
    };

    exportImageBytes = () => {
        if (!this._nativeRef) {
            return;
        }
        'exportImageBytes' in this._nativeRef && this._nativeRef.exportImageBytes();
    };

    render() {
        return (
            <RNInkCanvas {...this.props}
                         onChange={this.onChange}
                         onExport={this.onExport}
                         ref={this._onChangeNativeRef} />
        );
    }
}

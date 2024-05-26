import React from "react";
import { AudioRecorder } from "react-audio-voice-recorder";

const AudioMessages = () => {
  return (
    <>
      <AudioRecorder
        // onRecordingComplete={addAudioElement}
        audioTrackConstraints={{
          noiseSuppression: true,
          echoCancellation: true,
        }}
        onNotAllowedOrFound={(err) => console.table(err)}
        downloadOnSavePress={false}
        mediaRecorderOptions={{
          audioBitsPerSecond: 128000,
        }}
        showVisualizer={true}
      />
      <br />
    </>
  );
};

export default AudioMessages;

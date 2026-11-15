# REMEMBER DEFINITIONS

## DEFINITIONS

### Metronome
define :metronome do
  play :e2, amp: 0.5
end

### THEME VARIANTS ###
define :remember_theme_a do
  [:e4,:g4,:b4]
end

define :remember_theme_b do
  [:d4,:f4,:a4]
end

### BASS ###
### Remember Bass V1
define :remember_bass do
  with_synth :bass_foundation do
    
    play :e1,
      attack: 0.1,
      sustain: 3.5,
      release: 1.5,
      cutoff: 70,
      amp: 1
  end
  
end

### Remember Bass V2
define :remember_bass_v2 do
  with_fx :reverb,
    room: 0.7,
  mix: 0.3 do
    with_synth :bass_foundation do
      #play_chord [:e1, :b1],
      play_chord [:e0, :d1],
        attack: 0.05,
        sustain: 2.5,
        release: 4,
        cutoff: 1,
        amp: 1
    end
  end
end

### PIANO ###

### Remember Piano V1
define :remember_piano_v1 do |notes = [:e4,:g4,:b4], amp = 1|
  
  with_fx :reverb, room: 1, mix: 0.6 do
    with_synth :piano do
      play notes[0], amp: amp, release: 1.5
      sleep 1
      play notes[1], amp: amp, release: 2
      sleep 1
      play notes[2], amp: amp, release: 2
      sleep 2
    end
  end
end

### Remember Piano V2
define :remember_piano_v2 do
  with_fx :reverb, room: 1 do
    play_chord [:e4,:g4,:b4], release: 3, amp: 1
  end
end

### Remember Piano V3
define :remember_piano_v3 do |transpose = 0, amp = 1|
  notes = [
    note(:e4) + transpose,
    note(:g4) + transpose,
    note(:b4) + transpose
  ]
  
  with_fx :reverb, room: 1, mix: 0.6 do
    play notes[0],
      amp: amp,
      release: 1.5
    sleep 1
    play notes[1],
      amp: amp,
      release: 2
    sleep 1
    play notes[2],
      amp: amp,
      release: 2
    sleep 2
  end
end

### Remember Piano V4
define :remember_piano_v4 do |amp = 0.6|

  with_fx :reverb,
    room: 1,
    mix: 0.7 do

    with_synth :piano do

      [
        :b5,
        :a5,
        :g5,
        :e5,

        :d5,
        :b4,
        :a4,
        :e4
      ].each do |n|

        play n,
          amp: amp,
          release: 0.8

        sleep 1

      end
    end
  end
end
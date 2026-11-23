# itch.io Release Guide - `0.1.1-proto`

This guide prepares the first itch.io page for the `0.1.1-proto` test build.

## Release details

- **Title:** Silent Feel
- **Subtitle:** A psychological point-and-click horror prototype
- **Version:** `0.1.1-proto`
- **Developer:** Zig-Art Studio
- **Jam:** Game Off 2026
- **Theme:** SHIFT
- **Genres:** Horror, Adventure, Point-and-Click
- **Tags:** psychological horror, pixel art, point-and-click, mystery, narrative, 16-bit inspired
- **Platforms:** WebGL (recommended) and Windows x64
- **Status:** Prototype / released in development
- **Estimated duration:** 30-60 minutes
- **Pricing:** Free

## Files

1. Upload the WebGL build as **HTML / Play in browser**.
2. Compress the build contents, not the outer `0.1.1-proto` folder.
3. Make sure `index.html` is at the root of the ZIP.
4. Upload the Windows build as a separate downloadable ZIP.
5. Suggested filenames:
   - `SilentFeel-0.1.1-proto-WebGL.zip`
   - `SilentFeel-0.1.1-proto-Windows-x64.zip`

## Page configuration

- **Embed size:** `960 x 540`
- **Aspect ratio:** `16:9`
- **Fullscreen button:** Enabled
- **Mobile friendly:** No
- **Accessibility:** No
- **Visibility:** Draft while testing; restricted/private while sharing with testers
- **Repository:** Link to the public GitHub repository

The embed must remain 16:9. The game uses a 640x360 internal render resolution, scaled by the browser or player.

## Page text

### Short description

> **Silent Feel** is a psychological horror point-and-click adventure created for Game Off 2026. Explore a liminal building, switch between two realities, uncover hidden memories, and discover what happened to your father.

### Full description

> A silent building. Two overlapping realities. Five memories someone tried to erase.
>
> Explore the rooms of an abandoned sanatorium, investigate objects, solve practical puzzles, and switch between the Normal reality and the Other Side using SHIFT. Things hidden in one reality may become reachable in the other, revealing new routes and fragments of a disturbing personal history.
>
> Version `0.1.1-proto` contains the complete playable route: Lobby, Hallway, Bedroom, Library, Basement, Forest, Revelation Letter, and ending.
>
> Recover optional Memories to change the outcome. Finding 0-2 Memories leads to the Bad Ending, 3-4 to the Neutral Ending, and all 5 to the True Ending.

### Controls

```text
Left Click  - Interact, use an item, collect, or change rooms
Right Click - Examine
SHIFT / HUD - Switch reality in supported rooms
Escape      - Close or advance narrative interfaces
Enter       - Close or advance narrative interfaces
Space       - Close or advance narrative interfaces
Restart     - Reset progress and return to the Lobby
```

### Features

- Fixed-camera point-and-click exploration
- SHIFT-based reality switching
- Connected rooms, inventory objects, flags, and puzzle progression
- Five optional Memories with hidden ending consequences
- A complete revelation, ending evaluation, and restart loop
- Three endings based on the number of Memories recovered
- Runtime HUD, inventory, story overlays, and Memory presentation

### Prototype notes

> This is a complete game-jam prototype, not the final release. Some narrative copy and visual assets are still being polished. The game currently uses an in-session progress system; there is no persistent save system.

### Browser notes

> The WebGL version is recommended for desktop browsers. Use fullscreen for the clearest presentation. A Windows x64 build is also available as a download.

### Minimum credits

```text
Developed by Zig-Art Studio
Created for Game Off 2026
Made with Unity 6000.6.9f1
```

Replace this block with a final contributor, asset, music, and font credit list if required.

## HTML description template

Paste this block into the itch.io description editor after enabling HTML mode.
Replace the GitHub URL if the repository is moved or published under a different location.

```html
<div>
  <p><strong>Silent Feel</strong> is a psychological horror point-and-click adventure created for <strong>Game Off 2026</strong>.</p>

  <p>A silent building. Two overlapping realities. Five memories someone tried to erase.</p>

  <p>
    Explore the rooms of an abandoned sanatorium, investigate objects, solve practical puzzles,
    and switch between the <strong>Normal reality</strong> and the <strong>Other Side</strong> using SHIFT.
    Things hidden in one reality may become reachable in the other, revealing new routes and
    fragments of a disturbing personal history.
  </p>

  <p>
    Version <strong>0.1.1-proto</strong> contains the complete playable route:
    <strong>Lobby, Hallway, Bedroom, Library, Basement, Forest, Revelation Letter, and ending.</strong>
  </p>

  <hr>

  <h2>Controls</h2>
  <table>
    <tbody>
      <tr>
        <td><strong>Left Click</strong></td>
        <td>Interact, use an item, collect, or change rooms</td>
      </tr>
      <tr>
        <td><strong>Right Click</strong></td>
        <td>Examine</td>
      </tr>
      <tr>
        <td><strong>SHIFT / HUD Button</strong></td>
        <td>Switch reality in supported rooms</td>
      </tr>
      <tr>
        <td><strong>Escape / Enter / Space</strong></td>
        <td>Close or advance narrative interfaces</td>
      </tr>
      <tr>
        <td><strong>Restart</strong></td>
        <td>Reset progress and return to the Lobby</td>
      </tr>
    </tbody>
  </table>

  <hr>

  <h2>Endings</h2>
  <p>Recover optional Memories to change the outcome:</p>
  <ul>
    <li><strong>0-2 Memories:</strong> Bad Ending</li>
    <li><strong>3-4 Memories:</strong> Neutral Ending</li>
    <li><strong>5 Memories:</strong> True Ending</li>
  </ul>

  <hr>

  <h2>Features</h2>
  <ul>
    <li>Fixed-camera point-and-click exploration</li>
    <li>SHIFT-based reality switching</li>
    <li>Connected rooms, inventory objects, flags, and puzzle progression</li>
    <li>Five optional Memories with hidden ending consequences</li>
    <li>A complete revelation, ending evaluation, and restart loop</li>
    <li>Three endings based on the number of Memories recovered</li>
  </ul>

  <hr>

  <h2>Prototype Notes</h2>
  <p>
    This is a complete game-jam prototype, not the final release.
    Some narrative copy and visual assets are still being polished.
    The game uses an in-session progress system and does not currently provide persistent saves.
  </p>

  <h2>Browser Notes</h2>
  <p>
    The WebGL version is recommended for desktop browsers. Use fullscreen for the clearest presentation.
    A Windows x64 build is also available as a download.
  </p>

  <hr>

  <h2>Content Notice</h2>
  <p>
    Psychological horror, themes of disappearance and death, dark environments,
    tense visual effects, and some disturbing imagery.
  </p>

  <hr>

  <p>
    <strong>Developed by Zig-Art Studio</strong><br>
    Created for <strong>Game Off 2026</strong><br>
    Made with <strong>Unity 6000.6.9f1</strong>
  </p>
</div>
```

Optional repository call-to-action:

```html
<p>
  <a href="https://github.com/LAS1819/gameoff2026-gamejam">
    View the source code on GitHub
  </a>
</p>
```

## Recommended content notice

The page should indicate:

- Psychological horror
- Themes of disappearance and death
- Dark environments and tense visual effects
- Some disturbing imagery

Avoid describing scene-specific revelations or the truth behind the mystery before the player discovers it.

## Release checklist

- [ ] Cover image and screenshots uploaded
- [ ] Title and subtitle reviewed
- [ ] Public description reviewed for spoilers
- [ ] WebGL build opens from the itch.io embed
- [ ] No black letterboxing appears outside the 16:9 play area
- [ ] Fullscreen button tested
- [ ] Browser cache tested in private/incognito mode
- [ ] Windows x64 ZIP downloads and launches
- [ ] Version `0.1.1-proto` is visible
- [ ] Controls are readable on the page
- [ ] Content notice is visible
- [ ] Genres and tags are configured
- [ ] Public GitHub repository link is present
- [ ] Lobby through Ending and Restart flow works from itch.io

## Related guides

- [WebGL and Windows build guide](./BUILD-0.1.1-PROTO-WEBGL-WINDOWS.md)

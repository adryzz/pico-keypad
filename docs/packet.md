# This file describes all the packets supported.

## 0x00 (Host Handshake)

- Length: 1 byte

This message is sent by the host to check if a valid device is plugged in.

## 0x01 (Client Handshake)

- Length: 1 byte

This message is sent by the client to tell the host it is a valid device.

## 0x02 (OK)

- Length: 1 byte

This message is sent by either the host or the client to signal that the sent data was received and parsed properly.

## 0x03 (Error)

- Length: 1 byte

This message is sent by either the host or the client to signal that the sent data wasn't received or parsed properly.

## 0x04 (Configuration Request)

- Length: 1 byte

This message is sent by the host to the client to request its configuration.

## 0x05 (Configuration)

- Length: 18+ bytes

This message is sent either by the host or the client to retrieve/set the configuration.

### Description
- 2 bytes (VID)
- 2 bytes (PID)
- 2 bytes (Friendly name length)
- ? bytes (Friendly name)
- 1 byte (Left key pin)
- 1 byte (Left key)
- 4 bytes (Left key debounce time)
- 1 byte (Right key pin)
- 1 byte (Right key)
- 4 bytes (Right key debounce time)

# Raname: pick anything randomly, not limited to names

Raname can help you to pick a name or anything randomly based on the list you provide.

## Development
### 1. Set up the Environment
Required Visual Studio components: **.NET Desktop Development**

### 2. Clone the Repository

`git clone https://github.com/PerssonBrown/Raname.git`

### 3. Open .sln with Visual Studio and Build!

`./Raname/Raname.sln`

## Usage

You need to add a configuration file in the same directory as the application, named `config.json`.

This is a sample:

```json
{
    "nameList": [
        "Alice",
        "Bob"
    ],
    "timeDelta": "50"
}
```

* **nameList**: the content rolling on the screan
* **timeDelta**: interval at which content rolling on the screen (milliseconds) 

## License

### Raname
This project is licensed under the MIT License.

### Newtonsoft.Json
This project uses the [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) library, which is licensed under the MIT License.

Copyright (c) James Newton-King.
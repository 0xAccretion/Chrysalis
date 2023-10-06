# Chrysalis: Cardano Serialization Library for .NET 🦋

[![.NET](https://github.com/0xAccretion/Chrysalis/actions/workflows/dotnet.yml/badge.svg)](https://github.com/0xAccretion/Chrysalis/actions/workflows/dotnet.yml)

Chrysalis is an open-source .NET library designed to facilitate the serialization and deserialization of Cardano blockchain data structures. With a strong focus on adhering to the Cardano standards and enhancing the .NET Cardano developer ecosystem, Chrysalis aims to provide developers with a reliable and consistent toolkit for working with Cardano.

🚧 **NOTE:** This library is currently a work in progress. Feedback and contributions are welcome!

## Features

- **Cardano Serialization**: Convert Cardano data structures to and from CBOR (Concise Binary Object Representation).
- **Bech32 Address Encoding/Decoding**: Handle Cardano addresses seamlessly.
- **Extensive Data Model Support**: Work with a wide range of Cardano data types, including Transactions, Assets, MultiAssets, and more.
- **Smart Contract Interaction**: Interact with Cardano smart contracts.
- **Cross-Platform Compatibility**: Use Chrysalis in any .NET project, including .NET Core, .NET Framework, Xamarin, and more.


## Roadmap 🚀

1. **(De)serialization Support**: Achieve complete serialization and deserialization for any Cardano data type described in CDDL https://github.com/input-output-hk/cardano-ledger/blob/master/eras/alonzo/test-suite/cddl-files/alonzo.cddl.
2. **Transaction Handling**: Introduce capabilities for building and signing Cardano transactions.
3. **Advanced Address Management**: Implement address generation, derivation, and other associated functionalities.

## Getting Started

To use Chrysalis in your .NET project:

1. `dotnet add package Chrysalis --version 0.0.6`
2. Example Usage
    
    CBOR (De)serialization
    ```csharp
    var originalTransaction = CborSerializerV2.FromHex<Transaction>(originalTransactionCborHex)!;

    var serializedTransaction = CborSerializerV2.Serialize(originalTransaction);

    var deserializedTransaction = CborSerializerV2.Deserialize<Transaction>(serializedTransaction);
    ```

    Bech32 Address Encoding/Decoding
    ```csharp
    var addressBech32 = "addr...";
    var addressObject = Address.FromBech32(addressBech32);
    var addressBech32Again = addressObject.ToBech32();
    var paymentKeyHash = addressObject.GetPaymentKeyHash();
    var stakeKeyHash = addressObject.GetStakeKeyHash();
    ```


## How to Contribute

Interested in contributing to Chrysalis? Great! We appreciate any help, be it in the form of code contributions, documentation, or even bug reports.

- **Fork and Clone**: Fork this repository, clone it locally, and set up the necessary development environment.
- **Branch**: Always create a new branch for your work.
- **Pull Request**: Submit a pull request once you're ready. Ensure you describe your changes clearly.
- **Feedback**: Wait for feedback and address any comments or suggestions.

## License

MIT License

Copyright (c) 2023 0xAccretion

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

---

Give your feedback, star the repository if you found it useful, and consider contributing to push the Cardano .NET ecosystem forward! 🌟


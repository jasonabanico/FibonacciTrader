# FibonacciTrader

A simple app that takes historical price from [CoinAPI](https://docs.coinapi.io/), sets TA indicators (SMA, RSI, MACD) using [TA-Lib](https://ta-lib.org/), and identifies [Fibonacci retracement and extension levels](https://www.investopedia.com/terms/f/fibonacciextensions.asp).

## Assumptions

- For simplicity, cycles are approximated to happen every 1400 days, starting from the birth of Bitcoin. This is the [Bitcoin halving](https://www.investopedia.com/bitcoin-halving-4843769) cycle. (Actual halving cycles do not have a fixed number of days.)
- Alt coins follow the same cycle, driven by the [Crypto Money Flow Cycle](https://rektcapital.substack.com/p/crypto-money-flow-cycle).
- Cycle begins with a bull market upward trend, leading to a peak, followed by a bear market downward retracement, and moving sideways or slightly upwards towards the end of the cycle, following a pattern similar to the Wall Street Cheat Sheet.

![Wall Street Cheat Sheet](https://i.pinimg.com/736x/32/c7/39/32c739ad0296dcb687a34de1df8f9f03.jpg)

## Sample Output

    ----------
    BTC REPORT
    ----------

    Cycle Tops:
      For cycle 1, top happened on 16/06/2016 at $767.17. [RSI: 88.11] [MACD: 11.93]
      For cycle 2, top happened on 16/12/2017 at $19,515.17. [RSI: 79.64] [MACD: 254.9]
      For cycle 3, top happened on 9/11/2021 at $66,933.56. [RSI: 67.92] [MACD: -130.43]

    Cycle Bottoms:
      For cycle 1, bottom happened on 2/08/2016 at $536.94. [RSI: 21.8] [MACD: -5.91]
      For cycle 2, bottom happened on 15/12/2018 at $3,184.33. [RSI: 27.51] [MACD: 28.69]
      For cycle 3, bottom happened on 21/11/2022 at $15,761.98. [RSI: 31.44] [MACD: -137.31]

    Cycle Retracement Crossings:
    - For cycle 2:
      Retracement level 0.236 of $15,036.31 crossed on 22/12/2017 at $14,114.53. [RSI: 47.82] [MACD: -282.45]
      Retracement level 0.382 of $12,265.49 crossed on 16/01/2018 at $11,500.91. [RSI: 36.83] [MACD: -250.08]
      Retracement level 0.5 of $10,026.06 crossed on 30/01/2018 at $9,974.18. [RSI: 35.65] [MACD: 3.32]
      Retracement level 0.618 of $7,786.63 crossed on 5/02/2018 at $6,893.69. [RSI: 26.29] [MACD: -198.54]
      Retracement level 0.786 of $4,598.28 crossed on 20/11/2018 at $4,377.63. [RSI: 9.78] [MACD: -135.4]
    - For cycle 3:
      Retracement level 0.236 of $51,888.74 crossed on 4/12/2021 at $49,236.52. [RSI: 31.34] [MACD: -288.65]
      Retracement level 0.382 of $42,581.35 crossed on 7/01/2022 at $41,546.33. [RSI: 29.06] [MACD: -295.87]
      Retracement level 0.5 of $35,058.94 crossed on 22/01/2022 at $35,055.92. [RSI: 20.33] [MACD: -229.62]
      Retracement level 0.618 of $27,536.54 crossed on 12/06/2022 at $26,645.56. [RSI: 33.38] [MACD: 85.23]
      Retracement level 0.786 of $16,826.67 crossed on 9/11/2022 at $15,864.00. [RSI: 24.35] [MACD: -108.46]

    Cycle Extension Crossings:
    - For cycle 2:
      Extension level 1.618 of $909.46 crossed on 23/12/2016 at $915.93. [RSI: 90.49] [MACD: 5.73]
      Extension level 2.618 of $1,139.68 crossed on 23/02/2017 at $1,179.87. [RSI: 79.28] [MACD: 7.65]
      Extension level 4.236 of $1,512.20 crossed on 3/05/2017 at $1,518.54. [RSI: 87.52] [MACD: 17.99]
      Extension level 6.854 of $2,114.94 crossed on 23/05/2017 at $2,260.28. [RSI: 85.39] [MACD: 22.81]
    - For cycle 3:
      Extension level 1.618 of $29,607.63 crossed on 2/01/2021 at $32,213.11. [RSI: 86.82] [MACD: 443.28]
      Extension level 2.618 of $45,938.47 crossed on 8/02/2021 at $46,489.79. [RSI: 74.84] [MACD: 475.82]

    Next Cycle Extensions:
      Extension level 1.618 is $98,557.59.
      Extension level 2.618 is $149,729.16.
      Extension level 4.236 is $232,524.77.
      Extension level 6.854 is $366,491.95.

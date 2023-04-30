# FibonacciTrader

A simple app that takes historical price from [CoinAPI](https://docs.coinapi.io/), sets TA indicators (SMA, RSI, MACD) using [TA-Lib](https://ta-lib.org/), and identifies [Fibonacci retracement and extension levels](https://www.investopedia.com/terms/f/fibonacciextensions.asp).

## Assumptions

This thesis stands on four pillars: computer science, macroeconomics, market psychology, and math.
- For simplicity, cycles are approximated to happen every 1400 days, starting from the birth of Bitcoin. This is the [Bitcoin halving](https://www.investopedia.com/bitcoin-halving-4843769) cycle. (Actual halving cycles do not have a fixed number of days.) 
- It may also be possible that the crypto 4-year cycle is in sync with a 4-year liquidity cycle, starting with the Fed's bailout during the GFC in October, 2008, the event that led Satoshi Nakamoto to create Bitcoin.
- Alt coins follow Bitcoin's cycle, driven by the [Crypto Money Flow Cycle](https://rektcapital.substack.com/p/crypto-money-flow-cycle).
- For blue-chip high caps, the cycle begins with a bull market upward trend, leading to a peak, followed by a bear market downward retracement, and moving sideways or slightly upwards towards the end of the cycle, following a pattern similar to the Wall Street Cheat Sheet below.
- Cycle tops will only be identified after the 1st half of the cycle has passed.

![Wall Street Cheat Sheet](https://i.pinimg.com/736x/32/c7/39/32c739ad0296dcb687a34de1df8f9f03.jpg)

## Process

- Retrieve price history from CoinAPI or from local file.
- Annotate each price with technical indicators (SMA, RSI, MACD).
- Calculate the start dates of each cycle, and, for each price point, mark the cycle.
- Identify the cycle tops (highest price) and bottoms (lowest price).
- Calculate the retracement and extension levels for each cycle.
  - Retracement: Multiply the upward length (swing high or current cycle top minus swing low or previous cycle bottom) with Fibonacci retracement levels, and subtract it from the current cycle top.
  - Extension: Multiply the upward length (swing high or current cycle top minus swing low or previous cycle bottom) with Fibonacci extension levels, and add it to the current cycle bottom.
- Identify when retention and extension crossing occurred for each cycle.

## Strategy (not financial advice)

- Use history to determine the extension level to sell and the retracement level to buy again.
- From the outputs, it appears that the 0.786 retracement level can be an entry signal, and the 2.618 extension level can be an exit signal.

## Additional Features

- Price feed is cached as JSON text files to avoid unnecessary calls to CoinAPI.
- Technical analysis indicators may be extended to use the extensive function set of [TA-Lib](https://ta-lib.org/).

## Future Enhancements

- Email report. Regular (weekly) and alerts (extension crossing).

## Sample Output

- Bitcoin

    ----------
    BTC REPORT
    ----------

    Note: Cycle 1 is 2015 because that's the earliest record in CoinApi.

    Cycle Tops:
      For cycle 1 (2015), top happened on 16/06/2016 at $767.17. [RSI: 88.11] [MACD: 60.62 / Signal: 45.29]
      For cycle 2 (2016), top happened on 16/12/2017 at $19,515.17. [RSI: 79.64] [MACD: 2,492.78 / Signal: 2,161.49]
      For cycle 3 (2020), top happened on 9/11/2021 at $66,933.56. [RSI: 67.92] [MACD: 2,066.46 / Signal: 1,983.67]

    Cycle Bottoms:
      For cycle 1 (2015), bottom happened on 2/08/2016 at $536.94. [RSI: 21.80] [MACD: -14.84 / Signal: -3.57]
      For cycle 2 (2016), bottom happened on 15/12/2018 at $3,184.33. [RSI: 27.51] [MACD: -457.88 / Signal: -487.60]
      For cycle 3 (2020), bottom happened on 21/11/2022 at $15,761.98. [RSI: 31.44] [MACD: -948.59 / Signal: -806.70]

    Cycle Retracement Crossings:
    - For cycle 3 (2020):
      Retracement level 0.236 of $51,888.74 crossed on 4/12/2021 [25 days from top] at $49,236.52. [RSI: 31.34] [MACD: -1,903.86 / Signal: -1,271.70]
      Retracement level 0.382 of $42,581.35 crossed on 7/01/2022 [59 days from top] at $41,546.33. [RSI: 29.06] [MACD: -1,983.28 / Signal: -1,546.11]
      Retracement level 0.5 of $35,058.94 crossed on 22/01/2022 [74 days from top] at $35,055.92. [RSI: 20.33] [MACD: -2,402.69 / Signal: -1,919.02]
      Retracement level 0.618 of $27,536.54 crossed on 12/06/2022 [215 days from top] at $26,645.56. [RSI: 33.38] [MACD: -899.48 / Signal: -804.93]
      Retracement level 0.786 of $16,826.67 crossed on 9/11/2022 [365 days from top] at $15,864.00. [RSI: 24.35] [MACD: -208.75 / Signal: 166.97]

    Cycle Extension Crossings:
    - For cycle 3 (2020):
      Extension level 1.236 of $26,641.42 crossed on 28/12/2020 [744 days from bottom] at $27,040.26. [RSI: 77.50] [MACD: 2,027.36 / Signal: 1,644.96]
      Extension level 1.500 of $31,651.68 crossed on 2/01/2021 [749 days from bottom] at $32,213.11. [RSI: 86.82] [MACD: 2,795.14 / Signal: 2,222.16]
      Extension level 1.382 of $29,412.24 crossed on 3/01/2021 [750 days from bottom] at $32,970.09. [RSI: 87.70] [MACD: 3,037.52 / Signal: 2,385.23]
      Extension level 1.618 of $33,891.11 crossed on 5/01/2021 [752 days from bottom] at $33,999.32. [RSI: 83.55] [MACD: 3,302.12 / Signal: 2,685.79]
      Extension level 2.618 of $52,869.34 crossed on 19/02/2021 [797 days from bottom] at $55,932.11. [RSI: 78.51] [MACD: 4,753.91 / Signal: 3,893.18]

    Next Cycle Extensions:
      Extension level 1.236 is $94,556.02.
        Retracement level 0.236 is $75,960.63.
        Retracement level 0.382 is $64,456.70.
        Retracement level 0.5 is $55,159.00.
        Retracement level 0.618 is $45,861.31.
        Retracement level 0.786 is $32,623.91.
        Retracement level 1 is $15,761.98.
      Extension level 1.382 is $103,863.41.
        Retracement level 0.236 is $83,071.47.
        Retracement level 0.382 is $70,208.66.
        Retracement level 0.5 is $59,812.70.
        Retracement level 0.618 is $49,416.73.
        Retracement level 0.786 is $34,615.69.
        Retracement level 1 is $15,761.98.
      Extension level 1.500 is $111,385.82.
        Retracement level 0.236 is $88,818.59.
        Retracement level 0.382 is $74,857.51.
        Retracement level 0.5 is $63,573.90.
        Retracement level 0.618 is $52,290.29.
        Retracement level 0.786 is $36,225.48.
        Retracement level 1 is $15,761.98.
      Extension level 1.618 is $118,908.23.
        Retracement level 0.236 is $94,565.71.
        Retracement level 0.382 is $79,506.36.
        Retracement level 0.5 is $67,335.10.
        Retracement level 0.618 is $55,163.85.
        Retracement level 0.786 is $37,835.28.
        Retracement level 1 is $15,761.98.
      Extension level 2.618 is $182,657.45.
        Retracement level 0.236 is $143,270.12.
        Retracement level 0.382 is $118,903.38.
        Retracement level 0.5 is $99,209.72.
        Retracement level 0.618 is $79,516.05.
        Retracement level 0.786 is $51,477.61.
        Retracement level 1 is $15,761.98.
      Extension level 4.236 is $285,803.70.
        Retracement level 0.236 is $222,073.85.
        Retracement level 0.382 is $182,647.76.
       Retracement level 0.5 is $150,782.84.
        Retracement level 0.618 is $118,917.92.
        Retracement level 0.786 is $73,550.91.
        Retracement level 1 is $15,761.98.

- Ethereum

    ----------
    ETH REPORT
    ----------

    Note: Cycle 1 is 2015 because that's the earliest record in CoinApi.

    Cycle Tops:
      For cycle 1 (2015), top happened on 16/06/2016 at $20.81. [RSI: 82.10] [MACD: 1.76 / Signal: 1.20]
      For cycle 2 (2016), top happened on 13/01/2018 at $1,387.33. [RSI: 77.29] [MACD: 166.87 / Signal: 133.14]
      For cycle 3 (2020), top happened on 8/11/2021 at $4,780.41. [RSI: 69.91] [MACD: 242.83 / Signal: 225.06]

    Cycle Bottoms:
      For cycle 1 (2015), bottom happened on 2/08/2016 at $8.62. [RSI: 34.13] [MACD: -0.33 / Signal: 0.01]
      For cycle 2 (2016), bottom happened on 14/12/2018 at $82.98. [RSI: 25.91] [MACD: -18.45 / Signal: -19.71]
      For cycle 3 (2020), bottom happened on 18/06/2022 at $995.81. [RSI: 19.97] [MACD: -260.63 / Signal: -210.32]

    Cycle Retracement Crossings:
    - For cycle 3 (2020):
      Retracement level 0.236 of $3,671.81 crossed on 29/12/2021 [51 days from top] at $3,626.60. [RSI: 36.23] [MACD: -87.13 / Signal: -70.96]
      Retracement level 0.382 of $2,985.99 crossed on 21/01/2022 [74 days from top] at $2,571.68. [RSI: 24.11] [MACD: -229.07 / Signal: -195.40]
      Retracement level 0.5 of $2,431.69 crossed on 22/01/2022 [75 days from top] at $2,408.54. [RSI: 21.82] [MACD: -269.75 / Signal: -210.27]
      Retracement level 0.618 of $1,877.40 crossed on 26/05/2022 [199 days from top] at $1,787.81. [RSI: 31.10] [MACD: -211.29 / Signal: -220.52]
      Retracement level 0.786 of $1,088.23 crossed on 16/06/2022 [220 days from top] at $1,064.66. [RSI: 20.21] [MACD: -235.72 / Signal: -185.36]

    Cycle Extension Crossings:
    - For cycle 3 (2020):
      Extension level 1.236 of $1,787.07 crossed on 12/02/2021 [791 days from bottom] at $1,847.57. [RSI: 69.43] [MACD: 165.75 / Signal: 149.88]
      Extension level 1.382 of $1,988.37 crossed on 2/04/2021 [840 days from bottom] at $2,137.55. [RSI: 71.11] [MACD: 63.94 / Signal: 27.68]
      Extension level 1.500 of $2,151.05 crossed on 13/04/2021 [851 days from bottom] at $2,304.96. [RSI: 69.20] [MACD: 115.14 / Signal: 94.93]
      Extension level 1.618 of $2,313.74 crossed on 14/04/2021 [852 days from bottom] at $2,431.35. [RSI: 73.10] [MACD: 133.72 / Signal: 102.69]
      Extension level 2.618 of $3,692.46 crossed on 8/05/2021 [876 days from bottom] at $3,907.87. [RSI: 80.74] [MACD: 381.14 / Signal: 287.83]

    Next Cycle Extensions:
      Extension level 1.236 is $6,801.83.
        Retracement level 0.236 is $5,431.61.
        Retracement level 0.382 is $4,583.93.
        Retracement level 0.5 is $3,898.82.
        Retracement level 0.618 is $3,213.71.
        Retracement level 0.786 is $2,238.30.
        Retracement level 1 is $995.81.
      Extension level 1.382 is $7,487.65.
        Retracement level 0.236 is $5,955.58.
        Retracement level 0.382 is $5,007.77.
        Retracement level 0.5 is $4,241.73.
        Retracement level 0.618 is $3,475.70.
        Retracement level 0.786 is $2,385.07.
        Retracement level 1 is $995.81.
      Extension level 1.500 is $8,041.95.
        Retracement level 0.236 is $6,379.06.
        Retracement level 0.382 is $5,350.33.
        Retracement level 0.5 is $4,518.88.
        Retracement level 0.618 is $3,687.44.
        Retracement level 0.786 is $2,503.68.
        Retracement level 1 is $995.81.
      Extension level 1.618 is $8,596.25.
        Retracement level 0.236 is $6,802.54.
        Retracement level 0.382 is $5,692.88.
        Retracement level 0.5 is $4,796.03.
        Retracement level 0.618 is $3,899.18.
        Retracement level 0.786 is $2,622.30.
        Retracement level 1 is $995.81.
      Extension level 2.618 is $13,293.67.
        Retracement level 0.236 is $10,391.38.
        Retracement level 0.382 is $8,595.89.
        Retracement level 0.5 is $7,144.74.
        Retracement level 0.618 is $5,693.59.
        Retracement level 0.786 is $3,627.55.
        Retracement level 1 is $995.81.
      Extension level 4.236 is $20,894.11.
        Retracement level 0.236 is $16,198.11.
        Retracement level 0.382 is $13,292.96.
        Retracement level 0.5 is $10,944.96.
        Retracement level 0.618 is $8,596.96.
        Retracement level 0.786 is $5,254.05.
        Retracement level 1 is $995.81.

# FlexGrid

[![License](https://img.shields.io/github/license/soomin-kevin-sung/dotnet-flexgrid)](LICENSE.md)&nbsp;
![.NET](https://img.shields.io/badge/.NET-6.0-512BD4?style=flat)&nbsp;
![C#](https://img.shields.io/badge/.NET-C%23-007396?style=flat)&nbsp;
![WPF](https://img.shields.io/badge/WPF-DataGrid-007396?style=flat)&nbsp;

FlexGrid is a custom WPF DataGrid with convenient and useful features. When developing code using WPF, the Microsoft-supported DataGrid has limited functionality, such as nested and merged column headers and variable columns. However, with FlexGrid, your DataGrid development environment becomes significantly more convenient!

<br>

I'm proud to say that FlexGrid was fully built by me, and I'm excited to share it on GitHub :)

<br>

## Goal

- Create Customized DataGrid with convenient and useful features.
- Use FlexGrid to develop other WPF Programs

<br>

## Available Features

- [Using Bands Instead Of Columns](#using-bands-instead-of-columns)
- [Bands and Frozen Bands](#bands-and-frozen-bands)
- [Mergable Column Header (Band.Bands)](#mergable-column-header-bandbands)
- [Variable Columns (VirtualBand)](#variable-columns-virtualband)

<br>

## Using Bands Instead Of Columns

- (Writing Content...)

<br>

## Bands and Frozen Bands

- (Writing Content...)

<br>

## Mergable Column Header (Band.Bands)

- (Writing Content...)

<br>

## Variable Columns (VirtualBand)

- (Writing Content...)

<br>

## Samples

### Basic Sample

<p align="center">
  <img src="./resources/images/BasicSample.gif" alt="BasicSample.gif" />
  <br>
  &lt;BasicSample ScreenShot&gt;
</p>

- **BasicSample** shows the basic usage of FlexGrid.<br>
- You can know how to use **FlexGrid** the basically in this sample.

<br>

### Frozen Header Sample

<p align="center">
  <img src="./resources/images/FrozenHeaderSample.gif" alt="BasicSample.gif" />
  <br>
  &lt;FrozenHedaerSample ScreenShot&gt;
</p>

- FrozenHedaerSample shows how to using the frozen columns.<br>
- You can use **FlexGrid.FrozenBands** to add frozen columns.
- In this sample, the Name, BirthDate bands are Frozen Bands.

<br>

### Merged Header Sample

<p align="center">
  <img src="./resources/images/MergedHeaderSample.gif" alt="BasicSample.gif" />
  <br>
  &lt;MergedHedaerSample ScreenShot&gt;
</p>

- MergedHedaerSample shows how to merge column headers.<br>
- You can use **Band.Bands**(ex. TextBand.Bands, CheckBoxBand.Bands, etc.) to merge column headers.
- In this sample, you can see the Name, BirthDate, Address, and WebSite bands merged into the information band.

<br>

### VirtualBand Sample

<p align="center">
  <img src="./resources/images/VirtualBandSample.gif" alt="VirtualBandSample.gif" width=960px/>
  <br>
  &lt;VirtualBandSample ScreenShot&gt;
</p>

- VirtualBandSample shows how variable columns are implemented in FlexGrid.<br>
- You can use **VirtualBand**(ex. VirtualTextBand, VirtualComboBoxBand, VirtualCheckBoxBand, etc.) to show variable columns.
- In this Sample, you can see that the list of subject scores synchronizes with the subject list when you edit the subject list.

<p align="center">
  <img src="./resources/images/VirtualBandSample_ItemsSource_Image.png" alt="VirtualBandSample.gif" width=720px/>
  <br>
  &lt;Representation of ItemsSource&gt;
</p>

<br>

## Support

If you have any good ideas (such as new featrue, refactoring, improvement feature quality, etc), do not hesitate to let me know!<br>
You also can "Pull request" or request adding New Feature to the email below.
Thank you.

- E-mail : ssm0725@gmail.com

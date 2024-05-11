# eXtensionSharp

"eXtensionSharp" is a static class method extension.

This is a project that creates many useful methods based on null safe.

Refer to the test code for how to use it.

## Support
net7, net8

## Version
* 1.0.0.10
  * error modify - xcrypthmac -> fromHexToByte, fromHexToString xforeach loop error
  
* 1.0.0.9
  * remove - xForEach: remove thread.sleep
  * remove - xForEach: remove number type from to xforeach, use Enumerable.Range(from, to)  
  * modify - xForEach: change xforeach func item priority. (int Index, Generic T Item)
  * remove - XHttpExtension
  * add - xToDayOfWeek : day of week name (base by cultureinfo)
  * etc - XFileExtension change huge naming.
  * caution - xForEach can happen object null exception.  
    case 1 : use AOT, happen exception after serialize object.  

* 1.0.0.8
  * add - xToDictionary(this DataTable datatable) return IDictionary<string, object?>
  * add - xToDate(this DateTime dt, string format, CultureInfo culture) return string
  * modify - xBetween(this DateTime v, DateTime from, DateTime to) return bool
  * modify - project file - target framework
  * abort - XEnumBase<> - don't use more. use smartenum.
  * remove - FastDeepCloner - don't use more. use mapster.

* 1.0.0.6
  * add - static string xDistinct(this IEnumerable<string> items)
  * support net8

### Acknowledgements

[JetBrains](https://www.jetbrains.com/?from=eXtensionSharp) kindly provides `eXtensionSharp` with a free open-source
licence for their Resharper and Rider.

- **Resharper** makes Visual Studio a much better IDE
- **Rider** is fast & powerful cross platform .NET IDE

<img src="https://resources.jetbrains.com/storage/products/company/brand/logos/jb_beam.png" alt="JetBrains Logo (Main) logo." style="width:200px;height:200px;">

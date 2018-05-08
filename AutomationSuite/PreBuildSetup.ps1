param
(
	$ConfigFileName,
	$AppSettingsJson = $null,
	$GenConfigJson = $null
)

function Update-ConfigValue
(
	$ConfigFileName,
	$XPath,
	$Value
)
{
	Write-Debug "XPath [$XPath]"
	Write-Debug "New Value [$Value]"
	Write-Debug "Config File Name [$ConfigFileName]"
	[xml] $x = Get-Content $ConfigFileName
	$namespace = $x.DocumentElement.NamespaceURI
	if([string]::IsNullOrWhiteSpace($namespace) -eq $false)
	{
		Write-Debug "XML file has Name Space [$namespace]"
		$ns = New-Object System.Xml.XmlNamespaceManager($x.NameTable)
		$ns.AddNamespace("ns", $namespace)
		$node = $x.SelectSingleNode($XPath, $ns)
	}
	else
	{
		$node = $x.SelectSingleNode($XPath)
	}
	Write-Debug "XPath matching object found [$($node -ne $null)]"
	$node.Value = $Value
	$x.Save($ConfigFileName)
}

function Process-ConfigValue
(
	$JsonString,
	$isGenric,
	$xpathPattern
)
{
	Write-Debug "Indise [Process-ConfigValue] isGenric [$isGenric] xpathPattern [$xpathPattern]"
	if([string]::IsNullOrWhiteSpace($JsonString) -eq $false)
	{
		Write-Debug "Input App settings JSON [$JsonString]"
		Write-Debug "Formating JSON String"
		if($JsonString -match "[^\\]\\[^\\]")
		{
			$JsonString = $JsonString -replace "([^\\])\\([^\\])", "`$1\\`$2"
		}

		Write-Debug "Input App settings JSON after format [$JsonString]"

		$JsonStringObj = ConvertFrom-Json -InputObject $JsonString
		foreach($prop in $JsonStringObj.psobject.properties)
		{
			$xpath = $prop.Name
			if($isGenric -eq $false -and [string]::IsNullOrWhiteSpace($xpathPattern) -eq $false)
			{
				$xpath = $xpathPattern -f $prop.Name
			}

			Update-ConfigValue -ConfigFileName $ConfigFileName -XPath $xpath -Value $prop.value
		}
	}	
}

$DebugPreference = "Continue"
Process-ConfigValue -JsonString $AppSettingsJson -isGenric $false -xpathPattern "configuration/appSettings/add[@key=`"{0}`"]"
Process-ConfigValue -JsonString $GenConfigJson -isGenric $true
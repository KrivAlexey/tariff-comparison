# Tariff comparison service
Service for retrieving information about available electricity tariffs.
And for comparing tariffs by annual consumption kWh. 

# Getting Started
## Prerequisites
[.Net 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

## Installing
* clone the repo:   
    ```
    git clone https://github.com/KrivAlexey/tariff-comparison.git
    cd tariff-comparison
    ```
* install the dependencies:  
`dotnet restore`
## Build
`dotnet build`
## Run app 
`dotnet run --project .\TariffComparison\TariffComparison.csproj`
## Run tests
`dotnet test`

# REST API
Examples of http queries.

## Swagger
Swagger available at the endpoint `https://localhost:7286/index.html` when running with the `Development` environment.

## Get all tariffs
### Request
`curl -i -H 'Accept: applicaation/json' --insecure https://localhost:7286/tariffs`
### Response
```json
[
  {
    "name": "basic electricity tariff",
    "calculationModel": "base costs per month 5 € + consumption costs 22 cent/kWh",
    "calculationModelFormula": "60.0 + 0.22 * X",
    "calculationExamples": "\r\n• Consumption = 3500 kWh/year => Annual costs = 830 €/year (5€ * 12 months = 60 € base\r\ncosts + 3500 kWh/year * 22 cent/kWh = 770 € consumption costs)\r\n• Consumption = 4500 kWh/year => Annual costs = 1050 €/year (5€ * 12 months = 60 € base\r\ncosts + 4500 kWh/year * 22 cent/kWh = 990 € consumption costs)\r\n• Consumption = 6000 kWh/year => Annual costs = 1380 €/year (5€ * 12 months = 60 € base\r\ncosts + 6000 kWh/year * 22 cent/kWh = 1320 € consumption costs)"
  },
  {
    "name": "Packaged tariff",
    "calculationModel": "800 € for up to 4000 kWh/year and above 4000 kWh/year additionally 30cent/kWh.",
    "calculationModelFormula": "if X <= 4000.0 then 800.0 else 800.0 + (X - 4000.0) * 0.3",
    "calculationExamples": "\r\n• Consumption = 3500 kWh/year => Annual costs = 800 €/year\r\n• Consumption = 4500 kWh/year => Annual costs = 950 €/year (800€ + 500 kWh * 30 cent/kWh\r\n= 150 € additional consumption costs)\r\n• Consumption = 6000 kWh/year => Annual costs = 1400 €/year (800€ + 2000 kWh * 30\r\ncent/kWh = 600 € additional consumption costs)"
  }
]
```

## Compare tariffs 
### Request 
`curl --insecure -X 'POST' 'https://localhost:7286/tariffs/compare' -H 'accept: application/json'  -H 'Content-Type: application/json' -d '{\"AnnualConsumption\": 2500}'`
### Response
```json
[
  {
    "tariffName":"basic electricity tariff",
    "annualCost":610
  },
  {
    "tariffName":"Packaged tariff",
    "annualCost":800
  }
]
```
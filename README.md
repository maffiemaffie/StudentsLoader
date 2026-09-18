# Students Loader

Simple .NET Console App that loads and validates a JSON file containing student records.

# Usage

## Prerequisites

- Install the .NET SDK

## Setup

### Clone the Repo

```bash
git clone https://github.com/maffiemaffie/StudentsLoader.git
cd StudentsLoader
```

### Run the Project

```bash
dotnet run -- <json-filepath>
```

Sample JSON Files
|||
|-|-|
|`SampleData/all_valid.json`|Contains 20 valid student records|
|`SampleData/some_invalid.json`|Contains 16 valid student records and 4 invalid records|
|`SampleData/malformed.json`|A malformed JSON file|

## Assumptions
- The JSON input is a single array of records
- The input file is provided as a command line argument
- Valid JSON records can still have invalid fields

## Production Considerations
- **Meaningful Validators:** Validation is hardcoded as an example of possible validation parameters (min/max age, min/max name length). In production these would be decided based on the needs of the project and may be implemented in a way such that validators can be swapped out for modularity.
- **Optimized Validation:** For validation, a list of errors is generated for every record. If benchmarks found that this was using too much memory or slowing down the application, this could be optimized.
- **Optimized Validation:** For validation, the full list of records is iterated thorugh twice to separate out the valid and invalid records. The invalid records are then iterated through once again to list errors. In each pass, each record is validated. If benchmarks found that this was slowing down the application, validation could be cached or records could be validated in one pass.
- **More Robust Loading:** For the purposes of this short project, records with missing fields cause deserialization to fail and no records are loaded. In production, it may be worth considering whether individual invalid records should be rejected while allowing valid records to load.

## Research and AI Use
- I had previously used NewtonSoft for JSON deserialization, but used the newer `System.Text.Json` methods for this project. I referenced the Microsoft documentation and used AI to help debug errors.
- I used AI to rapidly generate sample data for testing.
- I referenced Microsoft docs for opening and reading files.
- I used AI to proofread my code to catch any mistakes I may have missed. 
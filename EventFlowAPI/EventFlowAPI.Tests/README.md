# EventFlowAPI.Tests

Characterization tests for the API. They describe what the application does today (including known defects) so a refactor can be checked for regressions.

## Requirements

- Docker. The tests start one throw-away SQL Server container (Testcontainers) for the whole run.
- To use an existing SQL Server instead (e.g. LocalDB), set `EVENTFLOW_TEST_SQL` to a connection string without a database name.
- Every test class creates a database `EventFlowTests_*` on that server, migrates it, shifts the seed dates to today, and drops it afterwards.

## Running

```
dotnet test EventFlowAPI/EventFlowAPI.Tests
```

Coverage of the application assemblies:

```
dotnet test EventFlowAPI/EventFlowAPI.Tests --settings EventFlowAPI/EventFlowAPI.Tests/coverage.runsettings --collect:"XPlat Code Coverage"
```

The test databases are created with the `Polish_CI_AS` collation and the tests run with the `pl-PL` culture, so sorting and formatting are the same on every machine.

## CI

`.github/workflows/event-flow-ci.yml` builds the API and the client and runs these tests on every push to `develop` (Ubuntu runner, SQL Server in a Testcontainers container, Central European time zone). When a test fails, the `.trx` results are uploaded as the `test-results` artifact.

## Expected data

The seeded responses, filters, sorting and paging are asserted in code: `Queries/QueryFilterTests.cs` lists the ids every query is expected to return (in order), `Queries/SeededResponseTests.cs` asserts the key fields of single records and who sees which records. Dates are compared relative to today. After an intended behaviour change, update the expected values in the test.

## Layout

- `Authorization` – access level of every endpoint and the 401/403 behaviour.
- `Queries` – seeded responses per role, filters, sorting and paging of every list endpoint.
- `Scenarios` – write flows: reservations, passes, hall rents, halls, events, festivals, lookups, photos (incl. payment types), files, statistics.
  - `CascadeTests` – side effects between halls, events, festivals, passes, rents and reservations.
  - `AtomicityTests` – rows left in the database after a rejected write (`DatabaseProbe` counts rows of every table).
  - `SoftUpdateTests` – updates of lookup types that are already referenced.
- `Infrastructure` – factory, database lifecycle, clients, helpers (`ScenarioWorld` builds users, events, festivals, halls, rents and reservations).

Authorization is checked once for all endpoints in `Authorization`; scenario tests only cover rules implemented inside services (ownership, roles checked in code).

Tests named `..._currently_...` document defects that exist today; change their expectation together with the fix.

Tests marked `Category=DatabaseSpecific` assert details of SQL Server and the current schema (foreign key errors, row counts per table, `OFFSET` errors). To check only the API contract, e.g. against another database implementation:

```
dotnet test EventFlowAPI/EventFlowAPI.Tests --filter Category!=DatabaseSpecific
```

Test classes run in parallel; each one has its own database and its own temporary folder for generated files (tickets, invoices, images), so nothing is written to the repository.

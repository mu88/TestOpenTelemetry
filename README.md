# How to run
1. Run `docker-compose up -d`. This will start the following services:
    - InfluxDB, running on http://localhost:8086
    - Telegraf, running on http://localhost:4317
    - Grafana, running on http://localhost:3000
2. Open Grafana and configure InfluxDB as data source with the following settings:
    - Data source: http://influxdb:8086
    - Disable BasicAuth
    - Org, bucket and token must be taken from the `.env` file

After a minute or so, you should see some metrics within Grafana. For example, you can execute the following Flux query in Grafana's _Explorer_ to see all .NET GC allocations:
```
from(bucket: "my-bucket")
  |> range(start: v.timeRangeStart, stop:v.timeRangeStop)
  |> filter(fn: (r) =>
    r._measurement == "process.runtime.dotnet.gc.allocations.size"
  )
```
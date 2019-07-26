# Http Alert

Service to make HTTP requests, enforce rules and alert if conditions are not met;

## Running with Docker

Customize your `http_alert.yml` and mount in volume

```
docker run --name http-alert -v ./http_alert.yml:/app/http_alert.yml -d thiagobarradas/http-alert:latest
```

https://hub.docker.com/r/thiagobarradas/http-alert

## http_alert.yml

```
# notification channels 
notifications:
  - name: team_a_channels 
    slack:
      url: https://hooks.slack.com/services/XXXXXXX
    pushover:
      token: xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
      user: xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
  - name: team_b_channels 
    slack:
      url: https://hooks.slack.com/services/YYYYYY
    pushover:
      token: yyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy
      user: yyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy
  - name: all_company 
    slack:
      url: https://hooks.slack.com/services/ZZZZZZZ

# monitoring config collection
http_configs:
  # first config
  - code: xxx-monitor
    url: http://xxx.yyy.com/resource
    user: my-basic-auth-user
    pass: my-basic-auth-pass
    timeout_seconds: 60
    headers:
      Application: HttpMonitoring
    stop_in_first_alert: false
    alert_in:
    - team_a_channels
    - all_company
    alert_when_exception: true

    # rules to validate http response
    rules:
    - condition: '"{state}" = "running"'
      error_title: 'Very high messages count for {vhost}/{name} '
      error_message: 'Messages is very high ({messages} messages) with publish rate {message_stats.publish_details.rate}/s and deliver rate {message_stats.deliver_get_details.rate}/s'
      alert_in:
      - team_a_channels
      alert_when_exception: true
    - condition: '"{state}" = "idle"'
      error_title: 'Very low messages count for {vhost}/{name} '
      error_message: 'Messages is very low ({messages} messages) with publish rate {message_stats.publish_details.rate}/s and deliver rate {message_stats.deliver_get_details.rate}/s'
      alert_in:
      - team_b_channels
      alert_when_exception: true
  
  # second config
  - code: yyy-monitor
    url: http://xxx.yyy.com/resource
    user: my-basic-auth-user
    pass: my-basic-auth-pass
    timeout_seconds: 60
    headers:
      Application: HttpMonitoring
    stop_in_first_alert: false
    alert_in:
    - team_a_channels
    - all_company
    alert_when_exception: true

    # rules to validate http response
    rules:
    - condition: '"{state}" = "running"'
      error_title: 'Very high messages count for {vhost}/{name} '
      error_message: 'Messages is very high ({messages} messages) with publish rate {message_stats.publish_details.rate}/s and deliver rate {message_stats.deliver_get_details.rate}/s'
      alert_in:
      - team_a_channels
      alert_when_exception: true
    - condition: '"{state}" = "idle"'
      error_title: 'Very low messages count for {vhost}/{name} '
      error_message: 'Messages is very low ({messages} messages) with publish rate {message_stats.publish_details.rate}/s and deliver rate {message_stats.deliver_get_details.rate}/s'
      alert_in:
      - team_b_channels
      alert_when_exception: true
```

see here: [http_alert.yml](HttpAlerts/http_alert.yml);

## Conditions /  Rules

We use [Flee](https://github.com/mparlak/Flee/wiki) to parser conditions expressions. Write conditions in your format using any json property path;

Samples:

- `"{state}" = "running"` string equals 
- `"{state}" != "running"` string not equals 
- `{messages.count} > 10` number gt
- `{messages.count} = 10` number equals
- `{messages.count} >= 10` number gte
- `{messages.count} < 10` number lt
- `{messages.count} <= 10` number lte
- `"{state}" = "running" OR {count} > 10` or operation
- `"{state}" = "running" AND {count} > 10` and operation

## How can I contribute?

Please, refer to [CONTRIBUTING](.github/CONTRIBUTING.md)

## Found something strange or need a new feature?

Open a new Issue following our issue template [ISSUE_TEMPLATE](.github/ISSUE_TEMPLATE.md)

## Did you like it? Please, make a donate :)

if you liked this project, please make a contribution and help to keep this and other initiatives, send me some Satochis.

BTC Wallet: `1G535x1rYdMo9CNdTGK3eG6XJddBHdaqfX`

![1G535x1rYdMo9CNdTGK3eG6XJddBHdaqfX](https://i.imgur.com/mN7ueoE.png)

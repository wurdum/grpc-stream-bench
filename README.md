# .NET gRPC Streams Benchmark

This repository contains code to benchmark gRPC streams in .NET. It has two dummy .NET applications - consumer and producer. Producer generates random records ~1KB each and sends them to consumer without any delay. Consumer receves the records and records the latency. Separate monitor instance runs Grafana and Prometheus to visualize the results.

# Results

| Producers | Consumers | Produced (messages/sec) | Latency 99th percentile |
| --- | --- | --- | --- |
| 1 node, 1 app, t3.large | 1 node, 1 app, t3.large | 75k | ~10ms |
| 1 node, 1 app, t3.xlarge | 1 node, 1 app, t3.xlarge | 75k | ~5ms |
| 1 node, 1 app, t3.xlarge | 1 node, 4 apps, t3.xlarge |  |  |

# How to run

* Install [Drone CLI](https://docs.drone.io/quickstart/cli/)
* Run pipeline `drone exec --secret-file ~/.drone/secrets.conf --env-file ~/.drone/env.conf --trusted` to provision infrastructure and run the benchmark
* Find dashboard URL in the Terraform output variable `monitor_public_dns_name`. Prepared dashboard name is `.NET Applications`

Drone secrets env file should contain the following variables:

```
AWS_ACCESS_KEY_ID=""
AWS_SECRET_ACCESS_KEY=""
AWS_BASTION_HOST="ec2-{IP}.eu-central-1.compute.amazonaws.com"
AWS_SSH_KEY="" # encoded SSH private key with "cat ~/.ssh/id_key.pem | openssl base64 | tr -d '\n'"

DOCKER_USER="" # Docker Hub username
DOCKER_PASS="" # Docker Hub access token
```

Drone variables env file:

```
AWS_REGION="eu-central-1"

TF_VAR_admin_static_ip=""
TF_VAR_ec2_ssh_key_name=""
TF_VAR_ec2_bastion_eip_id=""
```

To destroy provisioned infrastructure run `drone exec --secret-file ~/.drone/secrets.conf --env-file ~/.drone/env.conf --trusted --pipeline destroy`

resource "aws_route53_zone" "local" {
  name = var.domain_name

  vpc {
    vpc_id = aws_vpc.vpc.id
  }
}

resource "aws_route53_record" "consumer" {
  count   = var.consumer_count
  zone_id = aws_route53_zone.local.zone_id
  name    = "consumer${count.index}.${var.domain_name}"
  type    = "A"
  ttl     = "300"
  records = [
    aws_instance.consumer[count.index].private_ip
  ]
}

resource "aws_route53_record" "producer" {
  zone_id = aws_route53_zone.local.zone_id
  name    = "producer.${var.domain_name}"
  type    = "A"
  ttl     = "300"
  records = [
    aws_instance.producer.private_ip
  ]
}

resource "aws_route53_record" "monitor" {
  zone_id = aws_route53_zone.local.zone_id
  name    = "monitor.${var.domain_name}"
  type    = "A"
  ttl     = "300"
  records = [
    aws_instance.monitor.private_ip
  ]
}

output "consumer_r53_dns_name" {
  value = aws_route53_record.consumer[*].name
}

output "producer_r53_dns_name" {
  value = aws_route53_record.producer.name
}

output "monitor_r53_dns_name" {
  value = aws_route53_record.monitor.name
}

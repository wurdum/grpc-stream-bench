resource "aws_route53_zone" "local" {
  name = var.domain_name

  vpc {
    vpc_id = aws_vpc.vpc.id
  }
}

resource "aws_route53_record" "consumer" {
  zone_id = aws_route53_zone.local.zone_id
  name    = "consumer.${var.domain_name}"
  type    = "A"
  ttl     = "300"
  records = [
    aws_instance.consumer.private_ip
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

output "consumer_r53_dns_name" {
  value = aws_route53_record.consumer.name
}

output "producer_r53_dns_name" {
  value = aws_route53_record.producer.name
}

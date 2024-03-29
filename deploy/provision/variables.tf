variable "project_name" {
  type    = string
  default = "grpc_stream_bench"
}

variable "domain_name" {
  type    = string
  default = "app.domain"
}

variable "ingressrules" {
  type    = list(number)
  default = [22, 80, 443]
}

variable "consumer_count" {
  type    = number
  default = 4
}

variable "ec2_ami" {
  type = map(any)
  default = {
    owner = "099720109477"
    name  = "ubuntu/images/hvm-ssd/ubuntu-focal-20.04-amd64-server-20211129"
  }
}

variable "admin_static_ip" {
  type = string
}

variable "ec2_ssh_key_name" {
  type = string
}

variable "ec2_bastion_eip_id" {
  type = string
}

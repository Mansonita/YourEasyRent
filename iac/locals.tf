locals {
  bot_url              = google_cloud_run_service.bot.status[0].url
  bot_image_url        = "${var.registry_id}/${var.project_id}/${var.bot_name}:latest"
}
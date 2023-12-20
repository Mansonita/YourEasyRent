resource "google_cloud_run_service" "bot" {
  name     = var.bot_name
  location = var.region
  project  = var.project_id

  autogenerate_revision_name = true

  lifecycle {
    ignore_changes = [
      template[0].spec[5],
    ]
  }

  template {
    spec {
      containers {
        image = local.bot_image_url

        ports {
          container_port = var.port
        }

        resources {
          limits = {
            cpu    = "2000m"
            memory = "1Gi"
          }
        }

        env {
          name  = "TELEGRAM_BOT_TOKEN"
          value = var.bot_token
        }
      }
    }
  }

  traffic {
    percent         = 100
    latest_revision = true
  }

  depends_on = [google_project_service.cloud_run_googleapis_com]
}

# Allow unauthenticated users to invoke the service
resource "google_cloud_run_service_iam_member" "run_all_users" {
  service  = google_cloud_run_service.bot.name
  location = google_cloud_run_service.bot.location
  role     = "roles/run.invoker"
  member   = "allUsers"
} 
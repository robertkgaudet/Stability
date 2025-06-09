<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SpinningLogo.ascx.cs" Inherits="V1_UserControls_SpinningLogo" %>
<style>


        /* Already defined keyframes */
        @keyframes spin {
            from {
                transform: rotate(0deg);
            }

            to {
                transform: rotate(360deg);
            }
        }

        @keyframes spin-reverse {
            from {
                transform: rotate(0deg);
            }

            to {
                transform: rotate(-360deg);
            }
        }
		
        /* Base logo container */
/*        .rotating-logo {
            margin: 10px auto;
            height: 30px;
            height: 30px;
            cursor: pointer;
        }*/

		/* Base logo container */
		.rotating-logo {
			display: inline-block; /* ✅ Prevent full-width stretching */
			margin: 10px auto;
			width: auto; /* ✅ Allows it to shrink-wrap the image */
			height: auto;
			cursor: pointer;
			text-align: center;
		}

		.rotating-logo img {
				width: 30px;
				height: 30px;
				animation: spin 5s linear infinite;
				transition: all 0.3s ease-in-out;
			}

			/* Optional sizes */
			.logo-10 img {
				width: 10px;
				height: 10px;
			}
			.logo-20 img {
				width: 20px;
				height: 20px;
			}
			.logo-30 img {
				width: 30px;
				height: 30px;
			}
			.logo-40 img {
				width: 40px;
				height: 40px;
			}
			.logo-50 img {
				width: 50px;
				height: 50px;
			}
			.logo-60 img {
				width: 60px;
				height: 60px;
			}
			.logo-70 img {
				width: 70px;
				height: 70px;
			}
			.logo-80 img {
				width: 80px;
				height: 80px;
			}
			.logo-90 img {
				width: 90px;
				height: 90px;
			}
			.logo-100 img {
				width: 100px;
				height: 100px;
			}


        /* Base logo */
/*        .rotating-logo img {
            height: 30px;
            height: 30px;
            animation: spin 5s linear infinite;
            transition: all 0.3s ease-in-out;
        }*/

        /* Behaviors */
        .rotating-logo.fast img {
            animation-duration: 0.8s;
        }

        .rotating-logo.slow img {
            animation-duration: 12s;
        }

        .rotating-logo.reverse img {
            animation-name: spin-reverse;
        }

        .rotating-logo.paused img {
            animation-play-state: paused;
        }

        .rotating-logo.burst img {
            animation-duration: 0.4s;
        }
    </style>
    <script>
		document.addEventListener('DOMContentLoaded', function () {
			const logo = document.getElementById('<%=rotatingLogoDiv.ClientID%>');
			const behaviors = ['fast', 'slow', 'paused', 'burst', '']; // random fun
			let isReverse = false; // toggle tracker

			function spinLogo(event) {
				event.stopPropagation(); // 🚫 Prevent redirect via parent click

				// Remove previous classes
				logo.classList.remove('fast', 'slow', 'paused', 'burst', 'reverse');

				// Toggle direction
				isReverse = !isReverse;
				if (isReverse) {
					logo.classList.add('reverse');
				}

				// Apply one random spin behavior
				const behavior = behaviors[Math.floor(Math.random() * behaviors.length)];
				if (behavior) {
					logo.classList.add(behavior);
				}
			}

			// Trigger the spin on both events
			logo.addEventListener('click', spinLogo);
			logo.addEventListener('mouseenter', spinLogo);
		});
		</script>
		<%--<div class="rotating-logo logo-small" id="rotatingLogo"><img src="/V1/Images/pinwheel.png" alt="Stability Logo" /></div>--%>
		<div id="rotatingLogoDiv" runat="server" class="rotating-logo">
			<img src="/V1/Images/pinwheel.png" alt="Stability Logo" />
		</div>

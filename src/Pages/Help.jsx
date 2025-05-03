import { useState, useEffect } from "react";
import { Container } from "react-bootstrap";
import styles from './Help.module.css';  // استيراد ملف CSS موديل

export default function HelpPage() {
  const [openIndex, setOpenIndex] = useState({ category: -1, question: -1 });
  const [showScroll, setShowScroll] = useState(false);

  const checkScroll = () => {
    setShowScroll(window.pageYOffset > 400);
  };

  useEffect(() => {
    window.addEventListener("scroll", checkScroll);
    return () => window.removeEventListener("scroll", checkScroll);
  }, []);

  // FAQ data remains the same as before
  const faqData = [
    {
      category: "1. General Questions",
      items: [
        {
          question: "What is this website for?",
          answer:
            "This website helps you manage your monthly income and expenses to create a personalized budget. Our goal is to help you save money, avoid overspending, and achieve your financial goals.",
        },
        {
          question:
            "Why should I use this website instead of other budgeting tools?",
          answer:
            "Our website focuses on simplicity, personalized recommendations, and tools tailored to your financial goals. It's designed for users of all experience levels, from beginners to budgeting experts.",
        },
        {
          question: "How is my monthly income and expense data used?",
          answer:
            "We use your data solely to create personalized budgets and recommendations. Your information is not shared with third parties.",
        },
      ],
    },
    {
      category: "2. Getting Started",
      items: [
        {
          question: "How do I create an account?",
          answer:
            "Click on the 'Sign Up' button on the homepage, enter your details, and follow the prompts to set up your account.",
        },
        {
          question: "Can I change my account details later?",
          answer:
            "Yes, you can update your email, password, and other personal details in the account settings.",
        },
        {
          question: "How do I add my income and expenses?",
          answer:
            "On your dashboard, click on 'Add Income' or 'Add Expense,' fill out the details, and save. You can categorize each entry for better organization.",
        },
        {
          question: "Do I need to download anything?",
          answer:
            "No, everything is online. Just visit our website, sign in, and start managing your budget.",
        },
      ],
    },
    {
      category: "3. Troubleshooting",
      items: [
        {
          question: "I forgot my password. How do I reset it?",
          answer:
            "Click on 'Forgot Password' on the login page, and we'll send a reset link to your email.",
        },
        {
          question: "What should I do if I encounter a 'Server Error' message?",
          answer:
            "Wait a few minutes and try again. If the issue persists, contact support with details about the action you were performing.",
        },
        {
          question:
            "What should I do if the page freezes while I'm using the website?",
          answer:
            "Reload the page or restart your browser. If the problem persists, try using a different browser or device.",
        },
        {
          question: "Why does the website say my account doesn't exist?",
          answer:
            "Verify that you're using the correct email address. If you still have issues, contact support for assistance.",
        },
      ],
    },
  ];

  return (
    <div style={{ background: "var(--gradient-blue)", minHeight: "100vh" }}>
      <Container className="py-5 px-4">
        <h1 className={`mb-4 ${styles.mainTitle}`}>Help Page</h1>

        {faqData.map((category, catIndex) => (
          <div key={catIndex} className="mb-5">
            <h2 className={`mb-4 ${styles.categoryTitle}`}>{category.category}</h2>

            {category.items.map((item, itemIndex) => {
              const questionNumber = (itemIndex + 1)
                .toString()
                .padStart(2, "0");
              const isOpen =
                openIndex.category === catIndex &&
                openIndex.question === itemIndex;

              return (
                <div
                  key={itemIndex}
                  className={`faq-item mb-3 p-4 rounded-3 ${styles.faqItem}`}
                >
                  <div
                    className="d-flex justify-content-between align-items-center"
                    onClick={() =>
                      setOpenIndex(
                        isOpen
                          ? { category: -1, question: -1 }
                          : { category: catIndex, question: itemIndex }
                      )
                    }
                  >
                    <div className="d-flex align-items-center gap-4">
                      <span className={styles.numberBadge}>{questionNumber}</span>
                      <h3 className={`mb-0 ${styles.question}`}>
                        {item.question}
                      </h3>
                    </div>
                    <span className={styles.toggleIcon}>{isOpen ? "-" : "+"}</span>
                  </div>

                  {isOpen && (
                    <p className={`mt-3 mb-0 ${styles.answer}`}>
                      {item.answer}
                    </p>
                  )}
                </div>
              );
            })}
          </div>
        ))}

        {showScroll && (
          <button
            className={`btn btn-light position-fixed bottom-0 end-0 m-4 rounded-circle ${styles.scrollTop}`}
            onClick={() => window.scrollTo({ top: 0, behavior: "smooth" })}
          >
            ↑
          </button>
        )}
      </Container>
    </div>
  );
}
